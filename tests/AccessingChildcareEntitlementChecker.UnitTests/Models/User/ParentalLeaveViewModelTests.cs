using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.User;

public class ParentalLeaveViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ParentalLeaveViewModelTests()
    {
        _journeyState = new JourneyState();
        _localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType => _localizerFactory;
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        _journeyState.ParentalLeaveChildrenIds = new List<string> { "child1", "child2" };
        var backLink = "/previous-page";
        var returnTo = "some-return-to-value";
        var model = new ParentalLeaveViewModel(_journeyState, backLink, returnTo);
        Assert.Equal(backLink, model.BackLink);
        Assert.Equal(returnTo, model.ReturnTo);
        Assert.Equal(_journeyState.ParentalLeaveChildrenIds, model.ParentalLeaveChildrenIds);
        Assert.Equal(_journeyState.Children.Values.ToList(), model.Children);
    }

    [Fact]
    public void Validate_ReturnsErrorWhenNoneSelectedWithOptions()
    {
        var model = new ParentalLeaveViewModel()
        {
            ParentalLeaveChildrenIds =
            [
                ParentalLeaveViewModel.NoneSelectedValue,
                "SomeOtherValue",
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select which child you are on leave for, or 'None of these children'", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
    {
        var model = new ParentalLeaveViewModel()
        {
            ParentalLeaveChildrenIds = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select which child you are on leave for, or 'None of these children'", validationResults[0].ErrorMessage);
    }
}
