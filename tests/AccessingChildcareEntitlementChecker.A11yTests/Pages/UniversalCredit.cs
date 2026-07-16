using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class UniversalCreditPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task UniversalCreditPage_HasNoAccessibilityViolations()
    {
        await GoToUserUniversalCreditPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task UniversalCreditPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserUniversalCreditPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/universal-credit");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}