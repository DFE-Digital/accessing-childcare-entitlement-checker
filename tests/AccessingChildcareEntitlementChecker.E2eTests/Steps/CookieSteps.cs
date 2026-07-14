using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class CookieSteps(IPage page)
{
    [Then("the cookie banner is shown")]
    public async Task ThenTheCookieBannerIsShown()
    {
        await Expect(page.GetByRole(AriaRole.Region, new()
            {
                Name = "Cookies on Check if you are eligible for childcare funding"
            }))
            .ToBeVisibleAsync();
    }

    [When("I click the Accept button")]
    public async Task WhenIClickTheAcceptButton()
    {
        await page.GetByRole(AriaRole.Button, new()
            {
                Name = "Accept analytics cookies"
            })
            .ClickAsync();
    }

    [Then("the cookie banner is not shown")]
    public async Task ThenTheCookieBannerIsNotShown()
    {
        await Expect(page.GetByRole(AriaRole.Region, new()
            {
                Name = "Cookies on Check if you are eligible for childcare funding"
            }))
            .ToBeHiddenAsync();
    }

    [When("I click the Reject button")]
    public async Task WhenIClickTheRejectButton()
    {
        await page.GetByRole(AriaRole.Button, new()
            {
                Name = "Reject analytics cookies"
            })
            .ClickAsync();
    }

    [When("I click the link to change my cookie preferences")]
    public async Task WhenIClickTheLinkToChangeMyCookiePreferences()
    {
        await page.GetByRole(AriaRole.Link, new()
            {
                Name = "View cookies"
            })
            .ClickAsync();
    }

    [When("I click the Go back to the page you were looking at link")]
    public async Task WhenIClickTheGoBackToThePageYouWereLookingAtLink()
    {
        await page.GetByRole(AriaRole.Link, new()
            {
                Name = "Go back to the page you were looking at."
            })
            .ClickAsync();
    }
}
