using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ParentalLeavePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ParentalLeavePage_HasNoAccessibilityViolations()
    {
        await GoToUserParentalLeavePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ParentalLeavePage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserParentalLeavePage();
        await Continue();
        await ExpectPathAndQuery("/leave/parental-leave");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}