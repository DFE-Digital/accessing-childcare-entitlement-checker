using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.Tests.E2e;

public class Context
{
    private readonly IBrowser _browser;

    private Context(IBrowser browser, IPage page)
    {
        _browser = browser;
        Page = page;
    }

    public IPage Page { get; private set; }

    public static async Task<Context> CreateAsync(IBrowser browser)
    {
        var browserContext = await browser.NewContextAsync(new()
        {
            JavaScriptEnabled = false
        });

        var page = await browserContext.NewPageAsync();
        return new Context(browser, page);
    }

    public Uri Uri => new(Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/");

    public async Task DisposeAsync()
    {
        await Page.Context.CloseAsync();
    }
}
