using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerWeeklyEarningsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerWeeklyEarningsPage_HasNoAccessibilityViolations()
    {
        await AnswerPartnerAge();
        await AnswerPartnerPaidWorkStatus();
        await AnswerPartnerWorkStatus();
        await ExpectPathAndQuery($"/earnings/wage-partner");
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerWeeklyEarningsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerPartnerAge();
        await AnswerPartnerPaidWorkStatus();
        await AnswerPartnerWorkStatus();
        await ExpectPathAndQuery($"/earnings/wage-partner");
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}