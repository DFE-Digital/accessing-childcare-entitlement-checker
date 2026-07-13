using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class NationalityPageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/nationality";

    [Fact]
    public async Task NationalityPage_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task NationalityPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}