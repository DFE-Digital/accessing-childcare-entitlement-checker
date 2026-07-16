using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class HasPartnerPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task HasPartnerPage_HasNoAccessibilityViolations()
    {
        await GoToHasPartnerPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task HasPartnerPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToHasPartnerPage();
        await Continue();
        await ExpectPathAndQuery("/partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}