using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Tests.Unit.Models.Partner;

public class PartnerWorkStatusViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public PartnerWorkStatusViewModelTests()
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
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new PartnerWorkStatusViewModel
        {
            PartnerWorkStatus = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select how you would describe your partner's work status", validationResults[0].ErrorMessage);
    }
}
