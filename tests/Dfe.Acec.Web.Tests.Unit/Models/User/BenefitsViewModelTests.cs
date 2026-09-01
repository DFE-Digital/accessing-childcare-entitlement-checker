using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Tests.Unit.Models.User;

public class BenefitsViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public BenefitsViewModelTests()
    {
        var journeyState = new JourneyState();
        var localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void ValidateReturnsErrorWhenNoneSelectedWithOptions()
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
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
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
