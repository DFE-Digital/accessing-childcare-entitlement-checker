using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class SettledStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{

    [Fact]
    public async Task SettledStatusPage_HasNoAccessibilityViolations()
    {
        await AnswerUserNationalityCitizenOfEU();
        await EvaluatePage();
    }

    [Fact]
    public async Task SettledStatusPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerUserNationalityCitizenOfEU();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}