using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.Tests.E2e
{
    public class Context
    {
        private IPage? _page;

        public IPage Page => _page ?? throw new InvalidOperationException("Page has not been initialized. Ensure that the BeforeScenario hook has run.");

        public void SetPage(IPage page)
        {
            _page = page;
        }
    }
}