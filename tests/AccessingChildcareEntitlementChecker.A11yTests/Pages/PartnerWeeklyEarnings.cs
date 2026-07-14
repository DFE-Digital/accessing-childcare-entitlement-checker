using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerWeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerWeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await GoToPartnerWeeklyEarningsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerWeeklyEarningsPage();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}