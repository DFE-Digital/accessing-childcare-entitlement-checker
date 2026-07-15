using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class WorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WorkStatusPage_HasNoAccessibilityViolations()
    {
        await GoToUserWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WorkStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}