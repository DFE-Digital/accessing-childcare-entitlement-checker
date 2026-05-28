using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Children;

public class ChildSupportViewModelTests
{
    private readonly JourneyState _journeyState;
    private readonly IStringLocalizerFactory _localizerFactory;
    private readonly Func<Type, object> _serviceProviderFunc;
    private readonly Guid _childAId;
    private readonly Guid _childBId;

    public ChildSupportViewModelTests()
    {
        _journeyState = new JourneyState();
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
        _childBId = Guid.Parse("00000000-0000-0000-0000-00000000000b");
        var childA = new ChildState(_childAId, "Child A");
        _journeyState.AddChild(childA);

        _localizerFactory = AcecSubstitute.ForLocalizerFactory<ChildSupportViewModel>();
        _serviceProviderFunc = serviceType => _localizerFactory;
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
        var model = new ChildSupportViewModel(child)
        {
            ChildSupportOptions = []
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        Assert.Throws<InvalidOperationException>(() => model.Validate(validationContext).ToList());
    }

    [Fact]
    public void Validate_ReturnsErrorWhenNoneSelectedWithOptions()
    {
        Assert.True(_journeyState.TryGetChild(_childAId, out var child));
        var model = new ChildSupportViewModel(child)
        {
            ChildSupportOptions =
            [
                ChildSupport.PersonalIndependencePayment,
                ChildSupport.NoneOfTheseApply,
            ],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("You may not select 'no, none of these apply' with other options", validationResults[0].ErrorMessage);
    }

    [Fact]
    public void Validate_ReturnsErrorWhenOptionsAreEmpty()
    {
        Assert.True(_journeyState.TryGetChild(_childAId, out var child));
        var model = new ChildSupportViewModel(child)
        {
            ChildSupportOptions = [],
        };

        var validationContext = new ValidationContext(model);
        validationContext.InitializeServiceProvider(_serviceProviderFunc);

        var validationResults = model.Validate(validationContext).ToList();

        Assert.Single(validationResults);
        Assert.Equal("Select any support Child A gets, or select 'No, none of these apply'", validationResults[0].ErrorMessage);
    }
}
