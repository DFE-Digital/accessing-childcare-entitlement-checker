using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerWorkStatusPageHasNoAccessibilityViolations()
    {
        await GoToPartnerWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWorkStatusPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}