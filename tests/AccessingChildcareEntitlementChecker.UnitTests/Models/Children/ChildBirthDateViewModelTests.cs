using System.ComponentModel.DataAnnotations;
using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Children;

public class ChildBirthDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly ITodayFactory _dateTimeFactory;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;
    private readonly Guid _childAId;

    public ChildBirthDateViewModelTests()
    {
        _journeyState = new JourneyState();
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        var child = new ChildState(_childAId, "Child A");
        _journeyState.AddChild(child);
        _dateTimeFactory = Substitute.For<ITodayFactory>();
        _localizerFactory = Substitute.For<IStringLocalizerFactory>();

        var localizer = Substitute.For<IStringLocalizer<ChildBirthDateViewModel>>();
        var localizedString = new LocalizedString("Enter a date of birth in the past", "TEST");
        localizer["Enter a date of birth in the past"].Returns(localizedString);
        _localizerFactory.Create(typeof(ChildBirthDateViewModel)).Returns(localizer);

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
    public void Validate_ReturnsErrorForFutureDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.Today.Returns(DateOnly.FromDateTime(now));
        Assert.True(_journeyState.TryGetChild(_childAId, out var child));
        Assert.NotNull(child);

        var model = new ChildBirthDateViewModel(child)
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
