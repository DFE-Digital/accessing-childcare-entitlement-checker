using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class NationalityPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task NationalityPageHasNoAccessibilityViolations()
    {
        await GoToUserNationalityPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task NationalityPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserNationalityPage();
        await Continue();
        await ExpectPathAndQuery("/nationality");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
