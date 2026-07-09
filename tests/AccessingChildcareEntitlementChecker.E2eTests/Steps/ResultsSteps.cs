using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
public class ResultsSteps(IPage page)
{
    [Then(@"I {word} see the public funds warning")]
    public async Task ThenICanOrCannotSeeThePublicFundsWarning(string canOrCannot)
    {
        var shouldSeeWarning = canOrCannot == "can";
        var expectation = Expect(page.GetByText("You need to check if you can access public funds"));

        if (shouldSeeWarning)
        {
            await expectation.ToBeVisibleAsync();
        }
        else
        {
            await expectation.Not.ToBeVisibleAsync();
        }
    }

    [When("I click the details link for {string}")]
    public async Task WhenIClickTheDetailsLinkForString(string childName)
    {
        var text = $"View detailed information about {childName}'s childcare support";
        await page.GetByText(text).ClickAsync();
    }
}
