using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class BenefitsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task BenefitsPageHasNoAccessibilityViolations()
    {
        await GoToUserBenefitsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task BenefitsPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToUserBenefitsPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/benefits");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}