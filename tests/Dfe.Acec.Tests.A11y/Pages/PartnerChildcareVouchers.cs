using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

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