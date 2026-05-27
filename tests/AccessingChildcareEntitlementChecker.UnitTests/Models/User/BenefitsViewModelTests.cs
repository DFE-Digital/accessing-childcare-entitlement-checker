using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.User;

public class BenefitsViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public BenefitsViewModelTests()
    {
        _journeyState = new JourneyState();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory<BenefitsViewModel>();
        _serviceProviderFunc = serviceType => _localizerFactory;
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Validate_ReturnsErrorWhenNoneSelectedWithOptions()
    {
        var model = new BenefitsViewModel()
        {
            Benefits =
            [
                BenefitsOption.CarersAllowance,
                BenefitsOption.None,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any benefits you get, or select 'No, I do not get any of these benefits'", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new BenefitsViewModel()
        {
            Benefits = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any benefits you get, or select 'No, I do not get any of these benefits'", validationResults[0].ErrorMessage);
    }
}
