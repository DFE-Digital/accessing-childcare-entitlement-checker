using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildcareVouchersPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildcareVouchersPage_HasNoAccessibilityViolations()
    {
        await GoToUserChildcareVouchersPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildcareVouchersPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserChildcareVouchersPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-vouchers");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}