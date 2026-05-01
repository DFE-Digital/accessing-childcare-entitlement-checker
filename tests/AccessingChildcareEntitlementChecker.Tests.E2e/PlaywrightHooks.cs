
using System.Diagnostics;
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
        private static string? _videoDir;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            _videoDir = Path.Combine(
                Path.GetTempPath(),
                "playwright-videos",
                DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            Directory.CreateDirectory(_videoDir);

            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = !Debugger.IsAttached,
                SlowMo = Debugger.IsAttached ? 1000 : 500,
                // Software rendering — required for video capture in headless mode on Windows
                Args = ["--use-angle=swiftshader"]
            });
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            Assert.NotNull(_browser);
            Assert.NotNull(_videoDir);

            var browserContext = await _browser.NewContextAsync(new BrowserNewContextOptions
            {
                RecordVideoDir = _videoDir
            });
            var page = await browserContext.NewPageAsync();
            _context.SetPage(page);
        }

        [AfterScenario(Order = 100)]
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
