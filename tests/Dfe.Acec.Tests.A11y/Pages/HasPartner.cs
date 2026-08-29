using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class HasPartnerPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task HasPartnerPageHasNoAccessibilityViolations()
    {
        await GoToHasPartnerPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task HasPartnerPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToHasPartnerPage();
        await Continue();
        await ExpectPathAndQuery("/partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}