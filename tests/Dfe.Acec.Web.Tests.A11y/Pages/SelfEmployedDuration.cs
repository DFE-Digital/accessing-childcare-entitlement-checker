using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class SelfEmployedDurationPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task SelfEmployedDurationPageHasNoAccessibilityViolations()
    {
        await GoToUserSelfEmployedDurationPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task SelfEmployedDurationPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserSelfEmployedDurationPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/self-employed");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}