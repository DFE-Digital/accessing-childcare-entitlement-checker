using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class NationalityPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task NationalityPageHasNoAccessibilityViolations()
    {
        await GoToUserNationalityPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task NationalityPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserNationalityPage();
        await Continue();
        await ExpectPathAndQuery("/nationality");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}