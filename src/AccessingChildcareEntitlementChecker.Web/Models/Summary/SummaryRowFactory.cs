using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace AccessingChildcareEntitlementChecker.Web.Models.Summary;

/// <summary>
/// Dynamically build rows for a summary.
/// </summary>
/// <remarks>
/// All valid questions should have a value at this point.
/// So we can avoid determining question/branch validity and just pick out non-null values.
/// We might prefer in future to tighten this by evaluating whether a branch is applicable
/// with either a runtime check or conditionals when building the summary.
///
/// Most views now have viewmodels with `DisplayAttribute` for titles; and enums with `DisplayAttribute`
/// for values. Those that don't are handled with custom logic to extract the localised resources
/// from the relevant view resource files.
/// </remarks>
public class SummaryRowFactory
{
    private const string TitleResourceKey = "Title";
    private readonly List<SummaryRowViewModel> _viewModels;
    private readonly IStringLocalizerFactory _stringLocalizerFactory;

    public SummaryRowFactory(IModelMetadataProvider modelMetadataProvider, string controllerName, IStringLocalizerFactory stringLocalizerFactory)
    {
        ModelMetadataProvider = modelMetadataProvider;
        ControllerName = controllerName;
        _stringLocalizerFactory = stringLocalizerFactory;
        _viewModels = [];
    }

    public IReadOnlyList<SummaryRowViewModel> ViewModels => _viewModels;

    private IModelMetadataProvider ModelMetadataProvider { get; }

    private string ControllerName { get; }

    public SummaryRowFactory Add<TViewModel, TProperty>(
        Expression<Func<TViewModel, TProperty?>> viewModelProperty,
        Enum? value,
        string controllerActionName)
    {
        if (value == null)
        {
            return this;
        }

        var displayValue = GetEnumDisplayName(value);
        return Add(viewModelProperty, displayValue, controllerActionName);
    }

    public SummaryRowFactory Add<TViewModel, TProperty, TItem>(
        Expression<Func<TViewModel, TProperty?>> viewModelProperty,
        List<TItem> value,
        string controllerActionName)
        where TItem : Enum
    {
        if (value.Count == 0)
        {
            return this;
        }

        var displayValue = string.Join(", ", value.Select(i => GetEnumDisplayName(i)));
        return Add(viewModelProperty, displayValue, controllerActionName);
    }

    public SummaryRowFactory Add<TViewModel, TProperty>(
        Expression<Func<TViewModel, TProperty?>> viewModelProperty,
        DateOnly? value,
        string controllerActionName)
    {
        if (value == null)
        {
            return this;
        }

        var displayValue = value.Value.ToString("d MMMM yyyy");
        return Add(viewModelProperty, displayValue, controllerActionName);
    }

    public SummaryRowFactory AddParentalLeave(List<string> value, JourneyState journeyState)
    {
        if (value.Count == 0)
        {
            return this;
        }

        var childrenNames = journeyState.Children
            .Where(c => value.Contains(c.Key))
            .Select(c => c.Value.Name)
            .ToList();

        var displayValue = string.Join(", ", childrenNames);
        return Add((ParentalLeaveViewModel s) => s.ParentalLeaveChildrenIds, displayValue, nameof(UserController.ParentalLeave));
    }

    public SummaryRowFactory AddPartnerParentalLeave(List<string> value, JourneyState journeyState)
    {
        if (value.Count == 0)
        {
            return this;
        }

        var childrenNames = journeyState.Children
            .Where(c => value.Contains(c.Key))
            .Select(c => c.Value.Name)
            .ToList();

        var displayValue = string.Join(", ", childrenNames);
        return Add((PartnerParentalLeaveViewModel s) => s.PartnerParentalLeaveChildrenIds, displayValue, nameof(PartnerController.PartnerParentalLeave));
    }

    public SummaryRowFactory AddLocation(CountryOfResidence? countryOfResidence)
    {
        if (countryOfResidence == null)
        {
            return this;
        }

        var displayKey = countryOfResidence switch
        {
            CountryOfResidence.England => "Option_England",
            CountryOfResidence.Wales => "Option_Wales",
            CountryOfResidence.Scotland => "Option_Scotland",
            CountryOfResidence.NorthernIreland => "Option_NorthernIreland",
            _ => throw new InvalidOperationException($"Unexpected CountryOfResidence value: {countryOfResidence}")
        };

        var displayValue = GetResourceValueFromViewForLocale("Views.Home.Location", displayKey);
        var label = GetResourceValueFromViewForLocale("Views.Home.Location", TitleResourceKey);
        return Add(label, displayValue, "Location");
    }

    public SummaryRowFactory AddUserAge(AgeRange? userAge)
    {
        if (userAge == null)
        {
            return this;
        }

        var displayKey = userAge switch
        {
            AgeRange.UnderEighteen => "Option_Under18",
            AgeRange.EighteenToTwenty => "Option_18To20",
            AgeRange.TwentyOneOrOver => "Option_21OrOver",
            _ => throw new InvalidOperationException($"Unexpected AgeRange value: {userAge}")
        };

        var displayValue = GetResourceValueFromViewForLocale("Views.User.UserAge", displayKey);
        var label = GetResourceValueFromViewForLocale("Views.User.UserAge", TitleResourceKey);
        return Add(label, displayValue, "UserAge");
    }

    public SummaryRowFactory AddHasPartner(bool? hasPartner)
    {
        if (hasPartner == null)
        {
            return this;
        }

        // Comes from shared resources; rather than make an exception, we'll just hard code this for now.
        var displayValue = hasPartner switch
        {
            true => "Yes",
            false => "No",
            _ => throw new InvalidOperationException($"Unexpected HasPartner value: {hasPartner}")
        };

        var label = GetResourceValueFromViewForLocale("Views.User.HasPartner", TitleResourceKey);
        return Add(label, displayValue, "HasPartner");
    }

    public SummaryRowFactory AddPartnerAge(AgeRange? partnerAge)
    {
        if (partnerAge == null)
        {
            return this;
        }

        var displayKey = partnerAge switch
        {
            AgeRange.UnderEighteen => "Option_Under18",
            AgeRange.EighteenToTwenty => "Option_18To20",
            AgeRange.TwentyOneOrOver => "Option_21OrOver",
            _ => throw new InvalidOperationException($"Unexpected AgeRange value: {partnerAge}")
        };

        var displayValue = GetResourceValueFromViewForLocale("Views.Partner.PartnerAge", displayKey);
        var label = GetResourceValueFromViewForLocale("Views.Partner.PartnerAge", TitleResourceKey);
        return Add(label, displayValue, "PartnerAge");
    }

    private SummaryRowFactory Add(
        string displayName,
        string displayValue,
        string controllerActionName)
    {
        var vm = new SummaryRowViewModel(
            true,
            displayName,
            displayValue,
            ControllerName,
            controllerActionName);
        _viewModels.Add(vm);
        return this;
    }

    private SummaryRowFactory Add<TViewModel, TProperty>(
        Expression<Func<TViewModel, TProperty?>> viewModelProperty,
        string value,
        string controllerActionName)
    {
        var key = GetLabelLocalisationKey(typeof(TViewModel), viewModelProperty);

        var vm = new SummaryRowViewModel(
            false,
            key,
            value,
            ControllerName,
            controllerActionName);

        _viewModels.Add(vm);

        return this;
    }

    private static string GetEnumDisplayName(Enum value)
    {
        return value.GetType()!
            .GetMember(value.ToString())![0]
            .GetCustomAttributes(typeof(DisplayAttribute), inherit: false)!
            .Cast<DisplayAttribute>()!
            .First()
            .Name ?? value.ToString();
    }

    private string GetLabelLocalisationKey(Type viewModelType, LambdaExpression lambdaExpression)
    {
        var body = lambdaExpression.Body;
        if (body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert)
        {
            body = unary.Operand;
        }

        if (body is not MemberExpression memberExpression)
        {
            throw new InvalidOperationException("Expression must access a property.");
        }

        if (memberExpression.Member is not PropertyInfo propertyInfo)
        {
            throw new InvalidOperationException("Expression must access a property.");
        }

        var metadata = this.ModelMetadataProvider.GetMetadataForProperty(viewModelType, propertyInfo.Name);
        if (metadata.DisplayName == null)
        {
            throw new InvalidOperationException("SummaryRowFactory requires properties to have a DisplayAttribute with a Name specified.");
        }

        return metadata.DisplayName;
    }

    private string GetResourceValueFromViewForLocale(string viewName, string resourceKey)
    {
        var localizer = _stringLocalizerFactory.Create(viewName, typeof(SummaryController).Assembly.GetName().Name!);
        return localizer[resourceKey];
    }
}
