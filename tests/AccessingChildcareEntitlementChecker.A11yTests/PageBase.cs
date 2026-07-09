using System.Net;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.A11yTests;

public abstract class PageBase(ITestOutputHelper output) : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    protected IPage Page { get; private set; } = null!;

    protected static string ServiceUrl => Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252";

    private static readonly string[] Impacts =
    [
        "critical",
        "serious"
    ];

    protected virtual string? PageUrl => null;

    public async ValueTask InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        var browserName = Environment.GetEnvironmentVariable("PLAYWRIGHT_BROWSER")?.ToLowerInvariant() ?? "chromium";

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = true
        };

        _browser = browserName switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
            "webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
            _ => await _playwright.Chromium.LaunchAsync(launchOptions)
        };

        var password = Environment.GetEnvironmentVariable("TEST_BASIC_AUTH_PASSWORD");

        var contextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true
        };

        if (!string.IsNullOrEmpty(password))
        {
            contextOptions.HttpCredentials = new HttpCredentials
            {
                Username = "a11y",
                Password = password
            };
        }

        var context = await _browser.NewContextAsync(contextOptions);

        Page = await context.NewPageAsync();
    }

    protected async Task GoToPage(HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        if (string.IsNullOrWhiteSpace(PageUrl))
        {
            throw new InvalidOperationException(
                $"{GetType().Name} does not define a PageUrl. " + "Either override PageUrl or navigate using a journey setup helper.");
        }

        var fullUrl = $"{ServiceUrl.TrimEnd('/')}/{PageUrl.TrimStart('/')}";

        var response = await Page.GotoAsync(fullUrl);

        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.Equal((int)expectedStatusCode, response?.Status);
    }

    protected async Task EvaluatePage()
    {
        var results = await Page.RunAxe();

        var violations = results
            .Violations
            .Where(v => Impacts.Contains(v.Impact))
            .ToArray();

        foreach (var violation in violations)
        {
            output.WriteLine($"{violation.Impact}: {violation.Description}");

            foreach (var node in violation.Nodes)
            {
                output.WriteLine(node.Html);
            }
        }

        Assert.True(violations.Length == 0, string.Join(Environment.NewLine, violations.Select(v => $"{v.Impact}: {v.Description}")));
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.DisposeAsync();
        }

        _playwright?.Dispose();

        GC.SuppressFinalize(this);
    }

    protected async Task Continue()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
    }

}