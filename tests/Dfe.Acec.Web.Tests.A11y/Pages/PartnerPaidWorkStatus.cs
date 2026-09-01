using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class PartnerPaidWorkStatusPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerPaidWorkStatusPageHasNoAccessibilityViolations()
    {
        await GoToPartnerPaidWorkStatusPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerPaidWorkStatusPageWithValidationErrorHasNoAccessibilityViolations()
    {
        await GoToPartnerPaidWorkStatusPage();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-partner");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}