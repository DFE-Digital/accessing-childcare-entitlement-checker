using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class WeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await AnswerUserAge();
        await AnswerUserNationality();
        await AnswerUserPaidWorkStatus();
        await AnswerUserWorkStatus();
        await ExpectPathAndQuery($"/earnings/wage");
        await EvaluatePage();
    }

    [Fact]
    public async Task WeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerUserAge();
        await AnswerUserNationality();
        await AnswerUserPaidWorkStatus();
        await AnswerUserWorkStatus();
        await ExpectPathAndQuery($"/earnings/wage");
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}