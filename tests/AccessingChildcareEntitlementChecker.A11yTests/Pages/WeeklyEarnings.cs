using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class WeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    protected override string PageUrl => "/earnings/wage";

    [Fact]
    public async Task WeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await AnswerUserAge();
        await EvaluatePage();
    }

    [Fact]
    public async Task WeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerUserAge();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}