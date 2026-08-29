using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Tests.Unit.Models.BornChildDetails;

public class ChildSupportViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildSupportViewModelTests()
    {
        _journeyState = new JourneyState
        {
            Children = { ["child-a"] = new Child("child-a", "Jack") }
        };

        var localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void ValidateThrowsWhenNoChild()
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
    public void ValidateReturnsErrorWhenNoneSelectedWithOptions()
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
        Assert.Equal("Select any support Jack gets, or select 'No, none of these apply'", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
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
