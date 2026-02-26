
using Microsoft.Playwright;
using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e
{
    [Binding]
    public class PlaywrightHooks(Context context)
    {
        private readonly Context _context = context;
        private static IPlaywright? _playwright;
        private static IBrowser? _browser;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new()
            {
                Headless = false
            });
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            Assert.NotNull(_browser);

            var browserContext = await _browser.NewContextAsync();
            var page = await browserContext.NewPageAsync();
            _context.SetPage(page);
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            Assert.NotNull(_context.Page);

            var page = _context.Page;
            await page.Context.CloseAsync();
        }

        [AfterTestRun]
        public static async Task AfterTestRun()
        {
            Assert.NotNull(_browser);
            Assert.NotNull(_playwright);

            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
