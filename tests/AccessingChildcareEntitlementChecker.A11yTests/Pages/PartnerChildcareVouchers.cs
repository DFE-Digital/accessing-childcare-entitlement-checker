using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerChildcareVouchersPageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/benefits/childcare-vouchers-partner";

    [Fact]
    public async Task PartnerChildcareVouchersPage_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerChildcareVouchersPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}