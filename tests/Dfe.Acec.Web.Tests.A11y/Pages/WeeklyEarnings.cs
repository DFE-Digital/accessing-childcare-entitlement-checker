using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class WeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WeeklyEarningsPageHasNoAccessibilityViolations()
    {
        await GoToUserWeeklyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WeeklyEarningsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserWeeklyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
