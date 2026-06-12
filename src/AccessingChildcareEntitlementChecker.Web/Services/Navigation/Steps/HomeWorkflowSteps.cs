using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class HomeWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.Location)
            .Permit(NavigationAction.Next, Page.ChildName);
    }
}
