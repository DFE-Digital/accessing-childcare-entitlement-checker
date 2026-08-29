using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class PartnerChildcareSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerChildcareSupportPageHasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareSupportPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerChildcareSupportPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerChildcareSupportPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}