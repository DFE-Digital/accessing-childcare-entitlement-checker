using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class WorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WorkStatusPageHasNoAccessibilityViolations()
    {
        await GoToUserWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WorkStatusPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}