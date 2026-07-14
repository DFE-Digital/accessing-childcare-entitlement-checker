using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class WeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await GoToUserWeeklyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserWeeklyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}