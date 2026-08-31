using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class UniversalCreditPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task UniversalCreditPageHasNoAccessibilityViolations()
    {
        await GoToUserUniversalCreditPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task UniversalCreditPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserUniversalCreditPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/universal-credit");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
