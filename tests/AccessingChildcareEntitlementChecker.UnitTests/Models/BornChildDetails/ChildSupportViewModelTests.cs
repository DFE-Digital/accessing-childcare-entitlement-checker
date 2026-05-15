using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.BornChildDetails;

public class ChildSupportViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildSupportViewModelTests()
    {
        _journeyState = new JourneyState
        {
            ChildName = "Jack",
        };

        _localizerFactory = AcecSubstitute.ForLocalizerFactory<ChildSupportViewModel>();
        _serviceProviderFunc = serviceType => _localizerFactory;
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Ctr_ThrowsOnEmptyChildName()
    {
        _journeyState.ChildName = null;
        Assert.Throws<ArgumentNullException>(() => new ChildSupportViewModel(_journeyState));
    }

    [Fact]
    public void Validate_ReturnsErrorWhenNoneSelectedWithOptions()
    {
        var model = new ChildSupportViewModel(_journeyState)
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
        var model = new ChildSupportViewModel(_journeyState)
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
