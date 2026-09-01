using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PartnerBenefitsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerBenefitsPageHasNoAccessibilityViolations()
    {
        await GoToPartnerBenefitsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerBenefitsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerBenefitsPage();
        await Continue();
        await ExpectPathAndQuery("/Partner/PartnerBenefits");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}