using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class ExpectedChildDetailsWorkflowStepsTests
{
    [Fact]
    public void ChildDueDate_Next_ReturnsExpectedChildRelationship()
    {
        var step = new ExpectedChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildDueDate);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ExpectedChildRelationship, machine.State);
    }

    [Fact]
    public void ChildDueDate_Back_ReturnsIsChildBorn()
    {
        var step = new ExpectedChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildDueDate);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.IsChildBorn, machine.State);
    }

    [Fact]
    public void ExpectedChildRelationship_Next_ReturnsCheckChildDetails()
    {
        var step = new ExpectedChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ExpectedChildRelationship);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.CheckChildDetails, machine.State);
    }

    [Fact]
    public void ExpectedChildRelationship_Back_ReturnsChildDueDate()
    {
        var step = new ExpectedChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ExpectedChildRelationship);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildDueDate, machine.State);
    }
}
