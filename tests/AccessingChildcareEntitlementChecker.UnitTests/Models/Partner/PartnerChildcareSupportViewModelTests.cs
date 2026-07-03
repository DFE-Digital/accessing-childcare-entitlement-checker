using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Partner;

public class PartnerChildcareSupportViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public PartnerChildcareSupportViewModelTests()
    {
        _journeyState = new JourneyState();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory();
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
        Assert.Equal("Select any of this childcare support your partner already gets, or select 'No, they do not get any of this childcare support'", validationResults[0].ErrorMessage);
    }


    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new PartnerChildcareSupportViewModel
        {
            PartnerChildcareSupport = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any of this childcare support your partner already gets, or select 'No, they do not get any of this childcare support'", validationResults[0].ErrorMessage);
    }
}
