using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.BornChildDetails;

public class ChildSupportViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildSupportViewModelTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children["child-a"] = new Child("child-a", "Jack");

        _localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType => _localizerFactory;
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Validate_ThrowsWhenNoChild()
    {
        var child = new Child("DOES-NOT-EXIST", "Child b");
        var model = new ChildSupportViewModel(child, "backLink")
        {
            ChildSupportOptions = []
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        Assert.Throws<InvalidOperationException>(() => model.Validate(validationContext).ToList());
    }

    [Fact]
    public void Validate_ReturnsErrorWhenNoneSelectedWithOptions()
    {
        Assert.True(_journeyState.Children.TryGetValue("child-a", out var child));
        var model = new ChildSupportViewModel(child, "backLink")
        {
            ChildSupportOptions =
            [
                ChildSupport.PersonalIndependencePayment,
                ChildSupport.NoneOfTheseApply,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("You may not select 'no, none of these apply' with other options", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
    {
        Assert.True(_journeyState.Children.TryGetValue("child-a", out var child));
        var model = new ChildSupportViewModel(child, "backLink")
        {
            ChildSupportOptions = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any support Jack gets, or select 'No, none of these apply'", validationResults[0].ErrorMessage);
    }
}
