using System.ComponentModel.DataAnnotations;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace Dfe.Acec.Tests.Unit.Models.BornChildDetails;

public class ChildBirthDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly ITodayFactory _dateTimeFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildBirthDateViewModelTests()
    {
        _journeyState = new JourneyState
        {
            Children = { ["child-a"] = new Child("child-a", "Child A") }
        };
        _dateTimeFactory = Substitute.For<ITodayFactory>();
        var localizerFactory = Substitute.For<IStringLocalizerFactory>();

        var localizer = Substitute.For<IStringLocalizer<ChildBirthDateViewModel>>();
        var localizedString = new LocalizedString("Enter a date of birth in the past", "TEST");
        localizer["Enter a date of birth in the past"].Returns(localizedString);

        localizerFactory.Create(typeof(ChildBirthDateViewModel)).Returns(localizer);

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
    public void ValidateReturnsErrorForFutureDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.Today.Returns(DateOnly.FromDateTime(now));
        Assert.True(_journeyState.Children.TryGetValue("child-a", out var child));
        Assert.NotNull(child);

        var model = new ChildBirthDateViewModel(child, "backLink")
        {
            ChildBirthDate = DateOnly.FromDateTime(now.AddDays(1)),
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("TEST", validationResults[0].ErrorMessage);
    }
}
