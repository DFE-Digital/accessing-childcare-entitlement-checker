using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation;

public interface IWorkflowStep
{
    void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId);
}
