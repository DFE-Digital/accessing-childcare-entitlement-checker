using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Tests.Unit.Models.Partner;

public class PartnerBenefitsViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public PartnerBenefitsViewModelTests()
    {
        var journeyState = new JourneyState();
        var localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState))
            {
                return journeyState;
            }

            if (serviceType == typeof(IStringLocalizerFactory))
            {
                return localizerFactory;
            }

            return null!;
        };
    }

    [Fact]
    public void ValidateReturnsErrorWhenNoneSelectedWithOptions()
    {
        var model = new PartnerBenefitsViewModel
        {
            PartnerBenefits =
            [
                PartnerBenefitsOption.CarersAllowance,
                PartnerBenefitsOption.None,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any benefits your partner gets, or select 'No, they do not get any of these benefits'", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new PartnerBenefitsViewModel
        {
            PartnerBenefits = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any benefits your partner gets, or select 'No, they do not get any of these benefits'", validationResults[0].ErrorMessage);
    }
}
