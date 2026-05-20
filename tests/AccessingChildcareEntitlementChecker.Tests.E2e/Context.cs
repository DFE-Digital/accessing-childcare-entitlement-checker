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
        var browserContext = await browser.NewContextAsync(new()
        {
            JavaScriptEnabled = false
        });

        var page = await browserContext.NewPageAsync();
        return new Context(page);
    }

    public Uri Uri => new(Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/");

    public async Task DisposeAsync()
    {
        await Page.Context.CloseAsync();
    }

    public async Task FlushPendingActions()
    {
        await Queue.FlushPendingActions(Page);
    }
}
