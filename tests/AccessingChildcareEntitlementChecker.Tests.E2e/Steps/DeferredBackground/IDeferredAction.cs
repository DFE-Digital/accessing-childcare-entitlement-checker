using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground
{
    public interface IDeferredAction
    {
        string Title { get; }

        Task Execute(IPage page);
    }
}
