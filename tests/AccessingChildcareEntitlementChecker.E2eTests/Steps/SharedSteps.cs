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
}
