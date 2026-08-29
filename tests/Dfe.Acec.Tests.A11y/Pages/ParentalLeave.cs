using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class ParentalLeavePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ParentalLeavePageHasNoAccessibilityViolations()
    {
        await GoToUserParentalLeavePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ParentalLeavePageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserParentalLeavePage();
        await Continue();
        await ExpectPathAndQuery("/leave/parental-leave");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}