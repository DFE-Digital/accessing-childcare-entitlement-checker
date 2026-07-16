using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildcareSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildcareSupportPage_HasNoAccessibilityViolations()
    {
        await GoToUserChildcareSupportPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildcareSupportPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserChildcareSupportPage();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}