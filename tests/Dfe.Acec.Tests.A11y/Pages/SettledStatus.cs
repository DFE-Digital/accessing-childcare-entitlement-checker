using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class SettledStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task SettledStatusPageHasNoAccessibilityViolations()
    {
        await GoToUserSettledStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task SettledStatusPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserSettledStatusPage();
        await Continue();
        await ExpectPathAndQuery("/nationality/settled-status");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}