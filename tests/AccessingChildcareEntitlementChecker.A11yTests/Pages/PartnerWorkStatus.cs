using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerWorkStatusPage_HasNoAccessibilityViolations()
    {
        await GoToPartnerWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWorkStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}