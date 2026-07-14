using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class YearlyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task YearlyEarningsPage_HasNoAccessibilityViolations()
    {
        await GoToUserYearlyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task YearlyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserYearlyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/adjusted-net-income");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}