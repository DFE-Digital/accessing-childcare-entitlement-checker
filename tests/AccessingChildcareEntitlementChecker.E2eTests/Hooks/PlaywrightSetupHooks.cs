using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace AccessingChildcareEntitlementChecker.E2eTests.Hooks;

[Binding]
internal class PlaywrightSetupHooks
{
    private static TestSettings _settings = null!;
    private static IPlaywright _playwrightInstance = null!;
    private static IBrowser _browser = null!;

    [BeforeTestRun]
    public static async Task BeforeTestRun()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        _settings = new TestSettings();
        configuration.GetSection(nameof(TestSettings)).Bind(_settings);

        _playwrightInstance = await Playwright.CreateAsync();

        var launchOptions = BrowserTypeLaunchOptions();

        _browser = await LaunchBrowser(_settings, _playwrightInstance, launchOptions);
    }

    [BeforeScenario(Order = 0)]
    public static void RegisterGlobalInstances(IObjectContainer objectContainer)
    {
        objectContainer.RegisterInstanceAs(_settings);
        objectContainer.RegisterInstanceAs(_browser);
    }

    [AfterTestRun]
    public static async Task AfterTestRun()
    {
        await _browser.CloseAsync();
        _playwrightInstance.Dispose();
    }

    private static async Task<IBrowser> LaunchBrowser(TestSettings settings, IPlaywright playwright, BrowserTypeLaunchOptions launchOptions)
    {
        return settings.Browser.ToLowerInvariant() switch
        {
            "firefox" => await playwright.Firefox.LaunchAsync(launchOptions),
            "webkit" => await playwright.Webkit.LaunchAsync(launchOptions),
            _ => await playwright.Chromium.LaunchAsync(launchOptions)
        };
    }

    private static BrowserTypeLaunchOptions BrowserTypeLaunchOptions()
    {
        return new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless,
            SlowMo = _settings.SlowMo
        };
    }
}
