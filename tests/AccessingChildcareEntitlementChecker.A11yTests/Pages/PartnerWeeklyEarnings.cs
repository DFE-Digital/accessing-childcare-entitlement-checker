using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerWeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    protected override string PageUrl => "/earnings/wage";

    [Fact]
    public async Task PartnerWeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await AnswerPartnerAge();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerPartnerAge();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}