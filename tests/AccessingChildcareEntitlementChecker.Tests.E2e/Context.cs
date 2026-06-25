using AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground;
using Microsoft.Playwright;
using System.Text;

namespace AccessingChildcareEntitlementChecker.Tests.E2e;

public class Context
{
    private Context(IPage page)
    {
        Queue = new DeferredActionQueue();
        Page = page;
    }

    public DeferredActionQueue Queue { get; }

    public IPage Page { get; private set; }

    public static async Task<Context> CreateAsync(IBrowser browser)
    {
        var password = Environment.GetEnvironmentVariable("TEST_BASIC_AUTH_PASSWORD");

        var contextOptions = new BrowserNewContextOptions
        {
            JavaScriptEnabled = false,
            UserAgent = "playwright-e2e"
        };

        if (!string.IsNullOrEmpty(password))
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"e2e:{password}"));
            contextOptions.ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Authorization"] = $"Basic {credentials}"
            };
        }

        var browserContext = await browser.NewContextAsync(contextOptions);

        var page = await browserContext.NewPageAsync();
        return new Context(page);
    }

    public static Uri Uri => new(Environment.GetEnvironmentVariable("TEST_URL") ?? "https://s279t01-web-fd-endpoint-hxg0g6g7fvgudvcx.a02.azurefd.net");

    public async Task DisposeAsync()
    {
        await Page.Context.CloseAsync();
    }

    public async Task FlushPendingActions()
    {
        await Queue.FlushPendingActions(Page);
    }
}
