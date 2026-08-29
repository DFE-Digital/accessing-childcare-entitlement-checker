using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Tests.Unit.Models.Partner;

public class PartnerChildcareSupportViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public PartnerChildcareSupportViewModelTests()
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
        var model = new PartnerChildcareSupportViewModel
        {
            PartnerChildcareSupport =
            [
                PartnerChildcareSupportOption.ChildcareVouchers,
                PartnerChildcareSupportOption.None,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any of this childcare support your partner already gets, or select 'No, they do not get any of these'", validationResults[0].ErrorMessage);
    }


    [Fact]
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new PartnerChildcareSupportViewModel
        {
            PartnerChildcareSupport = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any of this childcare support your partner already gets, or select 'No, they do not get any of these'", validationResults[0].ErrorMessage);
    }
}
