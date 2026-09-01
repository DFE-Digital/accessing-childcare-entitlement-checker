using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Tests.Unit.Models.User;

public class ParentalLeaveViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ParentalLeaveViewModelTests()
    {
        _journeyState = new JourneyState();
        var localizerFactory = AcecSubstitute.ForLocalizerFactory();
        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState))
            {
                return _journeyState;
            }

            if (serviceType == typeof(IStringLocalizerFactory))
            {
                return localizerFactory;
            }

            return null!;
        };
    }

    [Fact]
    public void ConstructorInitializesPropertiesCorrectly()
    {
        _journeyState.ParentalLeaveChildrenIds = ["child1", "child2"];
        var backLink = "/previous-page";
        var returnTo = "some-return-to-value";
        var model = new ParentalLeaveViewModel(_journeyState, backLink, returnTo);
        Assert.Equal(backLink, model.BackLink);
        Assert.Equal(returnTo, model.ReturnTo);
        Assert.Equal(_journeyState.ParentalLeaveChildrenIds, model.ParentalLeaveChildrenIds);
        Assert.Equal([.. _journeyState.Children.Values], model.Children);
    }

    [Fact]
    public void ValidateReturnsErrorWhenNoneSelectedWithOptions()
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
    public void ValidateReturnsErrorWhenOptionsAreEmpty()
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
