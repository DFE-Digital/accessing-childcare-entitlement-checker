using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models.ExpectedChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace Dfe.Acec.Tests.Unit.Models.ExpectedChildDetails;

public class ChildDueDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly ITodayFactory _dateTimeFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildDueDateViewModelTests()
    {
        _journeyState = new JourneyState
        {
            Children = { ["child-a"] = new Child("child-a", "Jack") }
        };
        _dateTimeFactory = Substitute.For<ITodayFactory>();
        var localizerFactory = AcecSubstitute.ForLocalizerFactory();

        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(ITodayFactory))
            {
                return _dateTimeFactory;
            }
            if (serviceType == typeof(IStringLocalizerFactory))
            {
                return localizerFactory;
            }
            return null!;
        };
    }

    [Fact]
    public void ValidateReturnsErrorForPastDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.Today.Returns(DateOnly.FromDateTime(now));
        Assert.True(_journeyState.Children.TryGetValue("child-a", out var child));
        var model = new ChildDueDateViewModel(child, "backLink")
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
