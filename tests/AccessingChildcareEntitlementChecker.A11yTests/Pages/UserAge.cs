using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class UserAgePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task UserAgePage_HasNoAccessibilityViolations()
    {
        await GoToUserAgePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task UserAgePage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToUserAgePage();
        await Continue();
        await ExpectPathAndQuery("/age/parent-age");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}