using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class BornChildDetailsWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.ChildBirthDate)
            .Permit(NavigationAction.Next, Page.ChildRelationship)
            .Permit(NavigationAction.Back, Page.IsChildBorn);

        machine.Configure(Page.ChildRelationship)
            .Permit(NavigationAction.Next, Page.ChildSupport)
            .Permit(NavigationAction.Back, Page.ChildBirthDate);

        machine.Configure(Page.ChildSupport)
            .Permit(NavigationAction.Next, Page.CheckChildDetails)
            .Permit(NavigationAction.Back, Page.ChildRelationship);
    }
}
