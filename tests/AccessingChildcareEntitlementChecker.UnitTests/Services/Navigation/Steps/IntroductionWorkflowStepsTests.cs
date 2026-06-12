using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class IntroductionWorkflowStepsTests
{
    [Fact]
    public void ChildName_Next_ReturnsIsChildBorn()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildName);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.IsChildBorn, machine.State);
    }

    [Fact]
    public void ChildName_Back_ReturnsLocation()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.ChildName);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.Location, machine.State);
    }

    [Fact]
    public void IsChildBorn_Back_ReturnsChildName()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.IsChildBorn);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildName, machine.State);
    }

    [Fact]
    public void IsChildBorn_Next_WhenChildIdIsNull_ReturnsCheckChildDetails()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.IsChildBorn);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.CheckChildDetails, machine.State);
    }

    [Fact]
    public void IsChildBorn_Next_WhenChildBorn_ReturnsChildBirthDate()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.IsChildBorn);
        var state = new JourneyState();
        var childId = "child-123";
        var child = new Child(childId, "Test Child") { BirthStatus = BirthStatus.Born };
        state.Children.Add(childId, child);

        step.Configure(machine, state, childId);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildBirthDate, machine.State);
    }

    [Fact]
    public void IsChildBorn_Next_WhenChildExpected_ReturnsChildDueDate()
    {
        var step = new IntroductionWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.IsChildBorn);
        var state = new JourneyState();
        var childId = "child-123";
        var child = new Child(childId, "Test Child") { BirthStatus = BirthStatus.Due };
        state.Children.Add(childId, child);

        step.Configure(machine, state, childId);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildDueDate, machine.State);
    }
}
