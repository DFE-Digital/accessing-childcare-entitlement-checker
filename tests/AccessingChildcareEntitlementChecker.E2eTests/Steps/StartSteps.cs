using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class StartSteps(IPage page)
{
    [Then("I should see a navigation bar with the service name {string}")]
    public async Task ThenIShouldSeeANavigationBarWithTheServiceName(string expectedServiceName)
    {
        var serviceLink = page.Locator(".govuk-service-navigation__service-name a");

        await Expect(serviceLink).ToBeVisibleAsync();
        await Expect(serviceLink).ToHaveTextAsync(expectedServiceName);
    }

    [Then("the navigation bar service name should link to the start page")]
    public async Task ThenTheNavigationBarServiceNameShouldLinkToTheStartPage()
    {
        var serviceLink = page.Locator(".govuk-service-navigation__service-name a");

        await Expect(serviceLink).ToHaveAttributeAsync("href", "/");
    }

    [Then("I should see a beta banner with the text {string}")]
    public async Task ThenIShouldSeeABetaBannerWithTheText(string expectedText)
    {
        var banner = page.Locator(".govuk-phase-banner");

        await Expect(banner.Locator(".govuk-tag")).ToHaveTextAsync("Beta");
        await Expect(banner.Locator(".govuk-phase-banner__text"))
            .ToContainTextAsync("This is a new service. Help us improve it");
        await Expect(banner.GetByRole(AriaRole.Link, new() { Name = "give your feedback (opens in new tab)" }))
            .ToBeVisibleAsync();
    }

    [Then("the beta banner should have a link to the qualtrix survey")]
    public async Task ThenTheBetaBannerShouldHaveALinkToTheQualtrixSurvey()
    {
        var link = page.Locator(".govuk-phase-banner a");

        await Expect(link).ToBeVisibleAsync();
        await Expect(link).ToHaveAttributeAsync(
            "href",
            "https://dferesearch.fra1.qualtrics.com/jfe/form/SV_8eotBOVwAQbdP8y"
        );
    }
}
