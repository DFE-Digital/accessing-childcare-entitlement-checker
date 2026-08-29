using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PartnerYearlyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerYearlyEarningsPageHasNoAccessibilityViolations()
    {
        await GoToPartnerYearlyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerYearlyEarningsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerYearlyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/adjusted-net-income-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}