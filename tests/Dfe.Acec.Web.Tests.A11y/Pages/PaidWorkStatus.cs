using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PaidWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PaidWorkStatusPageHasNoAccessibilityViolations()
    {
        await GoToUserPaidWorkPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PaidWorkStatusPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserPaidWorkPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
