using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class SharedSteps
{
    private Context _context;

    public SharedSteps(Context context)
    {
        _context = context;
    }

    [Then("I should see the hint text {string}")]
    public async Task ThenIShouldSeeTheHintText(string hintText)
    {
        await Expect(_context.Page.Locator(".govuk-hint"))
            .ToHaveTextAsync(hintText);
    }
}
