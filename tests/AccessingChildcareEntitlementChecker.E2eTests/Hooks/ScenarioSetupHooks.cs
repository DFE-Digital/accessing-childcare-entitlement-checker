using Microsoft.Playwright;
using Reqnroll;
using Reqnroll.BoDi;

namespace AccessingChildcareEntitlementChecker.E2eTests.Hooks;

[Binding]
internal class ScenarioSetupHooks(
    IObjectContainer objectContainer,
    IReqnrollOutputHelper outputHelper,
    FeatureContext featureContext,
    ScenarioContext scenarioContext)
{
    [BeforeScenario(Order = 1)]
    public async Task BeforeScenario()
    {
        var settings = objectContainer.Resolve<TestSettings>();
        var browser = objectContainer.Resolve<IBrowser>();

        var contextOptions = BrowserNewContextOptions(
            settings,
            featureContext.FeatureInfo.Title,
            scenarioContext.ScenarioInfo.Title);

        var browserContext = await browser.NewContextAsync(contextOptions);
        var page = await browserContext.NewPageAsync();

        page.Response += (_, response) =>
        {
            outputHelper.WriteLine($"[{featureContext.FeatureInfo.Title}] [{scenarioContext.ScenarioInfo.Title}] [{response.Status}] {response.Url}");
        };

        objectContainer.RegisterInstanceAs(browserContext);
        objectContainer.RegisterInstanceAs(page);
    }

    [AfterScenario]
    public async Task AfterScenario()
    {
        var browserContext = objectContainer.Resolve<IBrowserContext>();
        if (browserContext != null)
        {
            await browserContext.CloseAsync();
        }
    }

    private BrowserNewContextOptions BrowserNewContextOptions(
        TestSettings settings,
        string featureTitle,
        string scenarioTitle)
    {
        var javascriptEnabled = scenarioContext
            .ScenarioInfo
            .Tags
            .Contains("javascript-enabled") ? true : false;
        return new BrowserNewContextOptions
        {
            JavaScriptEnabled = javascriptEnabled,
            UserAgent = $"{settings.UserAgent}, {featureTitle}, {scenarioTitle}",
            BaseURL = settings.TestUrl,
            ExtraHTTPHeaders = ExtraHttpHeaders(settings)
        };
    }

    private static Dictionary<string, string>? ExtraHttpHeaders(TestSettings settings)
    {
        if (string.IsNullOrEmpty(settings.BasicAuthPassword))
        {
            return null;
        }

        var credentials = $"e2e:{settings.BasicAuthPassword}";
        var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

        return new Dictionary<string, string>
        {
            { "Authorization", $"Basic {encoded}" }
        };
    }
}
