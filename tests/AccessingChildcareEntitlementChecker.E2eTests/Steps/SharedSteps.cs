using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class SharedSteps(IPage page)
{
    [Then("I should see the hint text {string}")]
    public async Task ThenIShouldSeeTheHintTextString(string hintText)
    {
        await Expect(page.Locator(".govuk-hint"))
            .ToHaveTextAsync(hintText);
    }

    [Then("I should see a success banner that says {string}")]
    public async Task ThenIShouldSeeASuccessBannerThatSaysString(string p0)
    {
        var banner = page.Locator(".govuk-notification-banner--success");
        await Expect(banner).ToBeVisibleAsync();
        await Expect(banner.GetByRole(AriaRole.Heading, new LocatorGetByRoleOptions { Name = "Success" })).ToHaveTextAsync("Success");
        await Expect(banner.Locator(".govuk-notification-banner__content")).ToContainTextAsync(p0);
    }
}
