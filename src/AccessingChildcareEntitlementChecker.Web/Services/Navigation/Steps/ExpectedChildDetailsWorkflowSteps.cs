using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class ExpectedChildDetailsWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.ChildDueDate)
            .Permit(NavigationAction.Next, Page.ExpectedChildRelationship)
            .Permit(NavigationAction.Back, Page.IsChildBorn);

        machine.Configure(Page.ExpectedChildRelationship)
            .Permit(NavigationAction.Next, Page.CheckChildDetails)
            .Permit(NavigationAction.Back, Page.ChildDueDate);
    }
}
