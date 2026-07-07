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

        _settings = SetWorkflowEnvironmentVariables(_settings);

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

    private static TestSettings SetWorkflowEnvironmentVariables(TestSettings testSettings)
    {
        // TODO: Update workflows to align with test setting. This maintains compatibility with environment variables
        var envUrl = Environment.GetEnvironmentVariable("TEST_URL");
        if (!string.IsNullOrEmpty(envUrl)) testSettings.TestUrl = envUrl;

        var envPass = Environment.GetEnvironmentVariable("TEST_BASIC_AUTH_PASSWORD");
        if (!string.IsNullOrEmpty(envPass)) testSettings.BasicAuthPassword = envPass;

        var envBrowser = Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSER");
        if (!string.IsNullOrEmpty(envBrowser)) testSettings.Browser = envBrowser;

        return testSettings;
    }
}
