using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.User;

public class WorkStatusViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public WorkStatusViewModelTests()
    {
        _journeyState = new JourneyState();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory<WorkStatusViewModel>();
        _serviceProviderFunc = serviceType => _localizerFactory;
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
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
