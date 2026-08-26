using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

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