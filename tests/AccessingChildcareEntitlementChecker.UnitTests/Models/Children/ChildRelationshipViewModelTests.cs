using System.ComponentModel.DataAnnotations;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Children;

public class ChildRelationshipViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;
    private readonly Guid _childAId;
    private readonly Guid _childBId;

    public ChildRelationshipViewModelTests()
    {
        _journeyState = new JourneyState();
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        _childBId = Guid.Parse("00000000-0000-0000-0000-00000000000b");
        var child = new ChildState(_childAId, "Child A");
        _journeyState.AddChild(child);
        _localizerFactory = Substitute.For<IStringLocalizerFactory>();

        var localizer = Substitute.For<IStringLocalizer<ChildRelationshipViewModel>>();
        localizer["Select your relationship to {0}", "Child A"]
            .Returns(new LocalizedString("Select your relationship to {0}", "Select your relationship to Child A"));
        _localizerFactory.Create(typeof(ChildRelationshipViewModel)).Returns(localizer);

        _serviceProviderFunc = serviceType =>
        {
            if (serviceType == typeof(JourneyState)) return _journeyState;
            if (serviceType == typeof(IStringLocalizerFactory)) return _localizerFactory;
            return null!;
        };
    }

    [Fact]
    public void Validate_ThrowsWhenNoChild()
    {
        var child = new ChildState(_childBId, "Child B");
        var model = new ChildRelationshipViewModel(child);

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        Assert.Throws<InvalidOperationException>(() => model.Validate(validationContext).ToList());
    }

    [Fact]
    public void Validate_ReturnsErrorWithChildNameWhenNoRelationshipSelected()
    {
        Assert.True(_journeyState.TryGetChild(_childAId, out var child));
        var model = new ChildRelationshipViewModel(child);
        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select your relationship to Child A", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsNoErrorsWhenRelationshipSelected()
    {
        Assert.True(_journeyState.TryGetChild(_childAId, out var child));
        var model = new ChildRelationshipViewModel(child)
        {
            Relationship = Relationship.Parent
        };
        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Empty(validationResults);
    }
}
