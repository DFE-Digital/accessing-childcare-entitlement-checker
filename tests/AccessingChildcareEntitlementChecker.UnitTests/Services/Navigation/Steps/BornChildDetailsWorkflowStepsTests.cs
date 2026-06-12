using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class BornChildDetailsWorkflowStepsTests
{
    [Fact]
    public void ChildBirthDate_Next_ReturnsChildRelationship()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildBirthDate);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildRelationship, machine.State);
    }

    [Fact]
    public void ChildBirthDate_Back_ReturnsIsChildBorn()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildBirthDate);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.IsChildBorn, machine.State);
    }

    [Fact]
    public void ChildRelationship_Next_ReturnsChildSupport()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildRelationship);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildSupport, machine.State);
    }

    [Fact]
    public void ChildRelationship_Back_ReturnsChildBirthDate()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildRelationship);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildBirthDate, machine.State);
    }

    [Fact]
    public void ChildSupport_Next_ReturnsCheckChildDetails()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildSupport);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.CheckChildDetails, machine.State);
    }

    [Fact]
    public void ChildSupport_Back_ReturnsChildRelationship()
    {
        var step = new BornChildDetailsWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildSupport);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildRelationship, machine.State);
    }
}
