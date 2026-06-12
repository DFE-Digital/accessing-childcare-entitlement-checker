using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class HomeWorkflowStepsTests
{
    [Fact]
    public void Configure_SetsLocationToChildNameOnNext()
    {
        // Arrange
        var step = new HomeWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.Location);
        var state = new JourneyState();

        // Act
        step.Configure(machine, state, null);

        // Assert
        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildName, machine.State);
    }
}
