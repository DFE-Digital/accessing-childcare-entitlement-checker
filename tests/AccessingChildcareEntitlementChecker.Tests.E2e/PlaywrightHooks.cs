
using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;
using System.Diagnostics;

namespace AccessingChildcareEntitlementChecker.Tests.E2e;

[Binding]
public class PlaywrightHooks(IObjectContainer objectContainer)
{
    private readonly IObjectContainer _objectContainer = objectContainer;
    private Context? _context;

    private static IPlaywright? _playwright;
    private static IBrowser? _browser;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        _playwright = await Playwright.CreateAsync();

        var browserName = Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSER")?.ToLowerInvariant();
        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = !Debugger.IsAttached,
            SlowMo = Debugger.IsAttached ? 1000 : 0
        };

        _browser = browserName switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
            "webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
            _ => await _playwright.Chromium.LaunchAsync(launchOptions)
        };
    }

    [BeforeScenario]
    public async Task BeforeScenario()
    {
        Assert.NotNull(_browser);

        _context = await Context.CreateAsync(_browser);
        _objectContainer.RegisterInstanceAs(_context);
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        Assert.NotNull(_context);

        await _context.DisposeAsync();
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
