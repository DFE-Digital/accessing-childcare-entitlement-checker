using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerChildcareSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerChildcareSupportPage_HasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareSupportPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerChildcareSupportPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareSupportPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}