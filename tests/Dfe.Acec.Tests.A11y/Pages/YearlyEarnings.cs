using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class YearlyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task YearlyEarningsPageHasNoAccessibilityViolations()
    {
        await GoToUserYearlyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task YearlyEarningsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserYearlyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/adjusted-net-income");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}