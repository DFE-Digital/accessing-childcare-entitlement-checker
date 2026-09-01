using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Tests.Unit.Models.User;

public class WorkStatusViewModelTests
{
    private readonly Func<Type, object> _serviceProviderFunc;

    public WorkStatusViewModelTests()
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
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new WorkStatusViewModel()
        {
            WorkStatus = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select your work status", validationResults[0].ErrorMessage);
    }
}
