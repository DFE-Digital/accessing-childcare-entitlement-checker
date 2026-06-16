using AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground;
using Microsoft.Playwright;

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
            contextOptions.HttpCredentials = new HttpCredentials
            {
                Username = "e2e",
                Password = password
            };
        }

        var browserContext = await browser.NewContextAsync(contextOptions);

        var page = await browserContext.NewPageAsync();
        return new Context(page);
    }

    public static Uri Uri => new(Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/");

    public async Task DisposeAsync()
    {
        await Page.Context.CloseAsync();
    }

    public async Task FlushPendingActions()
    {
        await Queue.FlushPendingActions(Page);
    }
}
