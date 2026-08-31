using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PartnerAgePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerAgePageHasNoAccessibilityViolations()
    {
        await GoToPartnerAgePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerAgePageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerAgePage();
        await Continue();
        await ExpectPathAndQuery("/age/partner-age");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
