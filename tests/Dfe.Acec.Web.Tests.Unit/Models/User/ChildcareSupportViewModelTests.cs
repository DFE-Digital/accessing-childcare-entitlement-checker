using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Tests.Unit.Models.User;

public class ChildcareSupportViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildcareSupportViewModelTests()
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
        var model = new ChildcareSupportViewModel()
        {
            ChildcareSupport =
            [
                ChildcareSupportOption.ChildcareVouchers,
                ChildcareSupportOption.None,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any of this childcare support you already get, or select 'No, I do not get any of these'", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new ChildcareSupportViewModel()
        {
            ChildcareSupport = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any of this childcare support you already get, or select 'No, I do not get any of these'", validationResults[0].ErrorMessage);
    }
}
