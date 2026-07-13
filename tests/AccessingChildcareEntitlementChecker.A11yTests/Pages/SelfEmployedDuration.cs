using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class SelfEmployedDurationPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{

    [Fact]
    public async Task SelfEmployedDurationPage_HasNoAccessibilityViolations()
    {
        await AnswerUserWorkStatusSelfEmployed();
        await EvaluatePage();
    }

    [Fact]
    public async Task SelfEmployedDurationPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AnswerUserWorkStatusSelfEmployed();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}