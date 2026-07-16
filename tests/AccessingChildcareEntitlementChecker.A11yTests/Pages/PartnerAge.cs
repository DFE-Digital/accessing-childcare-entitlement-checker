using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class PartnerAgePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task PartnerAgePage_HasNoAccessibilityViolations()
    {
        await GoToPartnerAgePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task PartnerAgePage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPartnerAgePage();
        await Continue();
        await ExpectPathAndQuery("/age/partner-age");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}