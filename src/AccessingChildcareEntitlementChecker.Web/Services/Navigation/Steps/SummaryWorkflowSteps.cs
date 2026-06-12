using System.Linq;
using AccessingChildcareEntitlementChecker.Web.Models;
using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class SummaryWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.CheckChildDetails)
            .Permit(NavigationAction.Next, Page.UserAge)
            .PermitDynamic(NavigationAction.Back, () =>
            {
                var lastChild = (childId is not null && state.Children.TryGetValue(childId, out var fromChild))
                    ? fromChild
                    : state.Children.Values.LastOrDefault();

                if (lastChild == null)
                {
                    return Page.Location;
                }

                return lastChild.BirthStatus == BirthStatus.Born
                    ? Page.ChildSupport
                    : Page.ExpectedChildRelationship;
            });

        machine.Configure(Page.CheckAnswers)
            .Permit(NavigationAction.Next, Page.Results);
    }
}
