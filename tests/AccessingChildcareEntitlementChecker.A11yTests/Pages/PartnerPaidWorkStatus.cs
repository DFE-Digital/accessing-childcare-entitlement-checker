using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerPaidWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerPaidWorkStatusPage_HasNoAccessibilityViolations()
    {
        await GoToPartnerPaidWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerPaidWorkStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerPaidWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}