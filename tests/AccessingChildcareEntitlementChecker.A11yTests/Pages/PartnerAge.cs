using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerAgePageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/age/partner-age";

    [Fact]
    public async Task PartnerAgePage_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerAgePage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}