using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class CookieSteps(IPage page)
{
    [When("I click the Cookies link in the footer")]
    public async Task WhenIClickTheCookiesLinkInTheFooter()
    {
        var footer = page.Locator("footer");
        var cookiesLink = footer.GetByRole(AriaRole.Link, new()
        {
            Name = "Cookies"
        }).ClickAsync();
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

    [When("I click the Accept button")]
    public async Task WhenIClickTheAcceptButton()
    {
        await page.GetByRole(AriaRole.Button, new()
        {
            Name = "Accept analytics cookies"
        })
        .ClickAsync();
    }

    [When("the unselected cookie banner is shown")]
    [Then("the unselected cookie banner is shown")]
    public async Task ThenTheUnselectedCookieBannerIsShown()
    {
        var unselectedCookieBanner = page.Locator("#unselected-cookies-banner");
        await Expect(unselectedCookieBanner).ToBeVisibleAsync();
    }

    [When("the accepted cookie banner is shown")]
    public async Task ThenTheAcceptedCookieBannerIsShown()
    {
        var acceptedCookieBanner = page.Locator("#accepted-cookies-banner");
        await Expect(acceptedCookieBanner).ToBeVisibleAsync();
    }

    [When("the rejected cookie banner is shown")]
    public async Task ThenTheRejectedCookieBannerIsShown()
    {
        var rejectedCookieBanner = page.Locator("#rejected-cookies-banner");
        await Expect(rejectedCookieBanner).ToBeVisibleAsync();
    }

    [When("I click the Hide cookie message button")]
    public async Task WhenIClickTheHideCookieMessageButton()
    {
        var hideCookieMessageButton = page.GetByRole(AriaRole.Button, new()
        {
            Name = "Hide cookie message"
        });
        await hideCookieMessageButton.ClickAsync();
    }

    [Then("the cookie banner is not shown")]
    public async Task ThenTheCookieBannerIsNotShown()
    {
        var cookieBanner = page.Locator("#cookies-banner");
        await Expect(cookieBanner).ToBeHiddenAsync();
    }
}
