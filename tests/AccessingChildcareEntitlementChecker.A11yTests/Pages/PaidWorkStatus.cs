using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PaidWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PaidWorkStatusPage_HasNoAccessibilityViolations()
    {
        await GoToUserPaidWorkPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PaidWorkStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserPaidWorkPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}