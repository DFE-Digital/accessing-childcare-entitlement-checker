using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PartnerWeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerWeeklyEarningsPageHasNoAccessibilityViolations()
    {
        await GoToPartnerWeeklyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWeeklyEarningsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerWeeklyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
