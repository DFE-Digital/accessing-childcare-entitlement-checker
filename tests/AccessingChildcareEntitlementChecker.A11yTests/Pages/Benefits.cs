using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class BenefitsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task BenefitsPage_HasNoAccessibilityViolations()
    {
        await GoToUserBenefitsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task BenefitsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserBenefitsPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/benefits");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}