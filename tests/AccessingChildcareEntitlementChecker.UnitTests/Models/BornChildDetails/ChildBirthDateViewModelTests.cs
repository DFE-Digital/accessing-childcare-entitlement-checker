using System.ComponentModel.DataAnnotations;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.BornChildDetails;

public class ChildBirthDateViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IDateTimeFactory _dateTimeFactory;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildBirthDateViewModelTests()
    {
        _journeyState = new JourneyState();
        _dateTimeFactory = Substitute.For<IDateTimeFactory>();
        _localizerFactory = Substitute.For<IStringLocalizerFactory>();

        var localizer = Substitute.For<IStringLocalizer<ChildBirthDateViewModel>>();
        var localizedString = new LocalizedString("Error_ChildBirthDateInFuture", "TEST Child birth date cannot be in the future.");
        localizer["Error_ChildBirthDateInFuture"].Returns(localizedString);
        _localizerFactory.Create(typeof(ChildBirthDateViewModel)).Returns(localizer);

        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(IDateTimeFactory))
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
    public void Ctr_ThrowsOnEmptyChildNName()
    {
        _journeyState.ChildName = null;
        Assert.Throws<ArgumentNullException>(() => new ChildBirthDateViewModel(_journeyState));
    }

    [Fact]
    public void Validate_ReturnsErrorForFutureDate()
    {
        var now = DateTime.UtcNow;
        _dateTimeFactory.UtcNow.Returns(now);
        var model = new ChildBirthDateViewModel(_journeyState)
        {
            ChildBirthDate = now.AddDays(1),
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);
        
        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("TEST Child birth date cannot be in the future.", validationResults[0].ErrorMessage);
    }
}