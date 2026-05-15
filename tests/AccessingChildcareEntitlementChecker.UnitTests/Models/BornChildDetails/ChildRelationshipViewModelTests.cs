using System.ComponentModel.DataAnnotations;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.BornChildDetails;

public class ChildRelationshipViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;

    public ChildRelationshipViewModelTests()
    {
        _journeyState = new JourneyState { ChildName = "Jack" };
        _localizerFactory = Substitute.For<IStringLocalizerFactory>();

        var localizer = Substitute.For<IStringLocalizer<ChildRelationshipViewModel>>();
        localizer["Select your relationship to {0}", "Jack"]
            .Returns(new LocalizedString("Select your relationship to {0}", "Select your relationship to Jack"));
        _localizerFactory.Create(typeof(ChildRelationshipViewModel)).Returns(localizer);

        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Ctr_ThrowsOnEmptyChildName()
    {
        _journeyState.ChildName = null;
        Assert.Throws<ArgumentNullException>(() => new ChildRelationshipViewModel(_journeyState));
    }

    [Fact]
    public void Validate_ReturnsErrorWithChildNameWhenNoRelationshipSelected()
    {
        var model = new ChildRelationshipViewModel(_journeyState);
        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select your relationship to Jack", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsNoErrorsWhenRelationshipSelected()
    {
        var model = new ChildRelationshipViewModel(_journeyState)
        {
            Relationship = Relationship.Parent
        };
        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Empty(validationResults);
    }
}
