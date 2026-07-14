using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerBenefitsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerBenefitsPage_HasNoAccessibilityViolations()
    {
        await GoToPartnerBenefitsPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerBenefitsPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerBenefitsPage();
        await Continue();
        await ExpectPathAndQuery("/Partner/PartnerBenefits");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}