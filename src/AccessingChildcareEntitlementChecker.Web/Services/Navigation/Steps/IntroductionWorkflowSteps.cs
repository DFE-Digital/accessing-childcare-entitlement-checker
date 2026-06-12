using AccessingChildcareEntitlementChecker.Web.Models;
using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class IntroductionWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.ChildName)
            .Permit(NavigationAction.Next, Page.IsChildBorn)
            .Permit(NavigationAction.Back, Page.Location);

        machine.Configure(Page.IsChildBorn)
            .PermitDynamic(NavigationAction.Next, () =>
            {
                if (string.IsNullOrEmpty(childId)) return Page.CheckChildDetails;
                var child = state.GetChild(childId);
                return child?.BirthStatus == BirthStatus.Born ? Page.ChildBirthDate : Page.ChildDueDate;
            })
            .Permit(NavigationAction.Back, Page.ChildName);
    }
}
