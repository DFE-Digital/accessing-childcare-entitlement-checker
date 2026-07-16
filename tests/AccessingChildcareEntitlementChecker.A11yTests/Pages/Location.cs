using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class LocationPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task LocationPage_HasNoAccessibilityViolations()
    {
        await StartJourney();
        await ExpectPathAndQuery("/where-do-you-live");
        await EvaluatePage();
    }

    [Fact]
    public async Task LocationPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await StartJourney();
        await ExpectPathAndQuery("/where-do-you-live");
        await Continue();
        await ExpectPathAndQuery("/where-do-you-live");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}