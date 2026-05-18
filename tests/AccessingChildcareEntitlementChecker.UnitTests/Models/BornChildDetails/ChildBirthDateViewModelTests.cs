using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.BornChildDetails;

public class ChildBirthDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly ITodayFactory _dateTimeFactory;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildBirthDateViewModelTests()
    {
        _journeyState = new JourneyState();
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
    /*
    [Fact]
    public void Ctr_ThrowsOnEmptyChildName()
    {
        _journeyState.ChildName = null;
        Assert.Throws<ArgumentNullException>(() => new ChildBirthDateViewModel(_journeyState));
    }

    [Fact]
    public void Validate_ReturnsErrorForFutureDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.Today.Returns(DateOnly.FromDateTime(now));
        _journeyState.ChildName = "Child A";
        var model = new ChildBirthDateViewModel(_journeyState)
        {
            ChildBirthDate = DateOnly.FromDateTime(now.AddDays(1)),
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("TEST", validationResults[0].ErrorMessage);
    }*/
}
