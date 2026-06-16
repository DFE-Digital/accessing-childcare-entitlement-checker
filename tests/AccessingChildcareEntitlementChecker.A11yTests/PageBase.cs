using System.Net;
using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using Xunit;

namespace AccessingChildcareEntitlementChecker.A11yTests;

public abstract class PageBase(ITestOutputHelper output) : IAsyncLifetime
{
    private IPlaywright? _playwright;
    private IBrowser? _browser;

    protected IPage Page { get; private set; } = null!;

    protected static string ServiceUrl =>
        Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252";

    private static readonly string[] Impacts =
    [
        "critical",
        "serious"
    ];

    protected abstract string PageUrl { get; }

    public async ValueTask InitializeAsync()
    {
        _playwright = await Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = true
        });

        var context = await _browser.NewContextAsync(new()
        {
            IgnoreHTTPSErrors = true
        });

        Page = await context.NewPageAsync();
    }

    protected async Task GoToPage(HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
    {
        Console.WriteLine($"TEST_URL = {ServiceUrl}");
        Console.WriteLine($"PageUrl = {PageUrl}");

        var fullUrl = $"{ServiceUrl.TrimEnd('/')}/{PageUrl.TrimStart('/')}";

        Console.WriteLine($"Navigating to {fullUrl}");

        var response = await Page.GotoAsync(fullUrl);

        Console.WriteLine($"Status = {response?.Status}");
        Console.WriteLine($"Response URL = {response?.Url}");
        Console.WriteLine($"Final URL = {Page.Url}");


        //var response = await Page.GotoAsync($"{ServiceUrl}{PageUrl}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        Assert.True(
            response?.Status == (int)expectedStatusCode,
            $"URL={fullUrl}, Status={response?.Status}, ResponseUrl={response?.Url}, FinalUrl={Page.Url}");
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

        Assert.True(
            violations.Length == 0,
            string.Join(Environment.NewLine,
                violations.Select(v => $"{v.Impact}: {v.Description}")));
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
}