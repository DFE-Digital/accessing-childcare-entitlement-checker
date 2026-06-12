using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class SummaryWorkflowStepsTests
{
    [Fact]
    public void CheckChildDetails_Next_ReturnsUserAge()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.UserAge, machine.State);
    }

    [Fact]
    public void CheckChildDetails_Back_WhenNoChildrenExist_ReturnsLocation()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.Location, machine.State);
    }

    [Fact]
    public void CheckChildDetails_Back_WhenLastChildIsBorn_ReturnsChildSupport()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();
        var child = new Child("child1", "Born Child") { BirthStatus = BirthStatus.Born };
        state.Children.Add(child.ChildId, child);

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildSupport, machine.State);
    }

    [Fact]
    public void CheckChildDetails_Back_WhenLastChildIsExpected_ReturnsExpectedChildRelationship()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();
        var child = new Child("child1", "Expected Child") { BirthStatus = BirthStatus.Due };
        state.Children.Add(child.ChildId, child);

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ExpectedChildRelationship, machine.State);
    }

    [Fact]
    public void CheckChildDetails_Back_WithSpecificChildIdBorn_ReturnsChildSupport()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();

        var child1 = new Child("child1", "Expected Child") { BirthStatus = BirthStatus.Due };
        var child2 = new Child("child2", "Born Child") { BirthStatus = BirthStatus.Born };
        state.Children.Add(child1.ChildId, child1);
        state.Children.Add(child2.ChildId, child2);

        step.Configure(machine, state, "child2");

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildSupport, machine.State);
    }

    [Fact]
    public void CheckChildDetails_Back_WithSpecificChildIdExpected_ReturnsExpectedChildRelationship()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckChildDetails);
        var state = new JourneyState();

        var child1 = new Child("child1", "Expected Child") { BirthStatus = BirthStatus.Due };
        var child2 = new Child("child2", "Born Child") { BirthStatus = BirthStatus.Born };
        state.Children.Add(child1.ChildId, child1);
        state.Children.Add(child2.ChildId, child2);

        step.Configure(machine, state, "child1");

        Assert.True(machine.CanFire(NavigationAction.Back));
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.ExpectedChildRelationship, machine.State);
    }

    [Fact]
    public void CheckAnswers_Next_ReturnsResults()
    {
        var step = new SummaryWorkflowSteps();
        var machine = new StateMachine<Page, NavigationAction>(Page.CheckAnswers);
        var state = new JourneyState();

        step.Configure(machine, state, null);

        Assert.True(machine.CanFire(NavigationAction.Next));
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.Results, machine.State);
    }
}
