using System.Net;
using Deque.AxeCore.Playwright;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests;

public abstract class PageBase(ITestOutputHelper output) : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;
    private TestSettings _settings = null!;

    protected IPage Page { get; private set; } = null!;

    protected virtual string? PageUrl => null;

    public async ValueTask InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Local.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        _settings = new TestSettings();
        configuration.GetSection(nameof(TestSettings)).Bind(_settings);

        _playwright = await Playwright.CreateAsync();

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = _settings.Headless,
            SlowMo = _settings.SlowMo
        };

        _browser = _settings.Browser.ToLowerInvariant() switch
        {
            "firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
            "webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
            _ => await _playwright.Chromium.LaunchAsync(launchOptions)
        };

        var contextOptions = new BrowserNewContextOptions
        {
            IgnoreHTTPSErrors = true,
            UserAgent = _settings.UserAgent,
            BaseURL = _settings.TestUrl,
            ExtraHTTPHeaders = ExtraHttpHeaders(_settings)
        };

        var context = await _browser.NewContextAsync(contextOptions);

        Page = await context.NewPageAsync();
    }

    private static Dictionary<string, string>? ExtraHttpHeaders(TestSettings settings)
    {
        if (string.IsNullOrEmpty(settings.BasicAuthPassword))
        {
            return null;
        }

        var credentials = $"a11y:{settings.BasicAuthPassword}";
        var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(credentials));

        return new Dictionary<string, string>
        {
            { "Authorization", $"Basic {encoded}" }
        };
    }

    protected async Task GoToPage(HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        if (string.IsNullOrWhiteSpace(PageUrl))
        {
            throw new InvalidOperationException(
                $"{GetType().Name} does not define a PageUrl. " + "Either override PageUrl or navigate using a journey setup helper.");
        }

        var response = await Page.GotoAsync(PageUrl);
        Assert.Equal((int)expectedStatusCode, response?.Status);
    }

    protected async Task EvaluatePage()
    {
        var results = await Page.RunAxe();

        var violations = results
            .Violations
            .Where(v => _settings.Impacts.Contains(v.Impact))
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

    protected async Task ExpectPathAndQuery(string expectedPathAndQuery)
    {
        await Expect(Page).ToHaveURLAsync(expectedPathAndQuery);

        var actualPathAndQuery = new Uri(Page.Url).PathAndQuery;

        Assert.Equal(expectedPathAndQuery, actualPathAndQuery);
    }

}