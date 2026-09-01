using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class ChildcareSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildcareSupportPageHasNoAccessibilityViolations()
    {
        await GoToUserChildcareSupportPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildcareSupportPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserChildcareSupportPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
