using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerChildcareVouchersPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerChildcareVouchersPageHasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareVouchersPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerChildcareVouchersPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareVouchersPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-vouchers-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}