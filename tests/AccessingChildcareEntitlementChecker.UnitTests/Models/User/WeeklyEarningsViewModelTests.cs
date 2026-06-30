using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.User;

public class WeeklyEarningsViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public WeeklyEarningsViewModelTests()
    {
        _journeyState = new JourneyState();
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
    public void Validate_ReturnsErrorWhenNoneSelected()
    {
        _journeyState.UserAge = AgeRange.UnderEighteen;
        _journeyState.WorkStatus = [WorkStatusOption.Apprentice];

        var model = new WeeklyEarningsViewModel()
        {
            WeeklyEarnings = null,
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select if you earn £128 a week or more before tax", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_Coverage_ThrowsIfNoJourneyState()
    {
        Func<Type, object> serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };

        var model = new WeeklyEarningsViewModel()
        {
            WeeklyEarnings = null,
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(serviceProviderFunc);

        Assert.Throws<InvalidOperationException>(() => model.Validate(validationContext).ToList());
    }
}
