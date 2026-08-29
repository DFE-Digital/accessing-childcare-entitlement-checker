using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class ChildcareVouchersPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildcareVouchersPageHasNoAccessibilityViolations()
    {
        await GoToUserChildcareVouchersPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildcareVouchersPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserChildcareVouchersPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-vouchers");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}