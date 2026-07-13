using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PaidWorkStatusPageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/work-status/work";

    [Fact]
    public async Task PaidWorkStatusPage_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PaidWorkStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}