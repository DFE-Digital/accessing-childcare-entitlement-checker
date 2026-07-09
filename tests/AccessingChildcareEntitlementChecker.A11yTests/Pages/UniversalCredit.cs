using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class UniversalCreditPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    protected override string PageUrl => "/benefits/universal-credit";

    [Fact]
    public async Task UniversalCreditPage_HasNoAccessibilityViolations()
    {
        await AnswerUserAge();
        await EvaluatePage();
    }

    [Fact]
    public async Task UniversalCreditPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}