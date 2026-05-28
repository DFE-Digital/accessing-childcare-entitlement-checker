using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Children;

public class ChildDueDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly ITodayFactory _dateTimeFactory;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildDueDateViewModelTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Children["child-a"] = new ChildState("Jack");
        _dateTimeFactory = Substitute.For<ITodayFactory>();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory<ChildDueDateViewModel>();

        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(ITodayFactory))
            {
                return _dateTimeFactory;
            }
            if (serviceType == typeof(IStringLocalizerFactory))
            {
                return _localizerFactory;
            }
            return null!;
        };
    }

    [Fact]
    public void Validate_ReturnsErrorForPastDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.Today.Returns(DateOnly.FromDateTime(now));
        Assert.True(_journeyState.TryGetChild("child-a", out var child));
        var model = new ChildDueDateViewModel(child)
        {
            ChildDueDate = DateOnly.FromDateTime(now.AddDays(-1)),
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Enter a due date in the future", validationResults[0].ErrorMessage);
    }
}
