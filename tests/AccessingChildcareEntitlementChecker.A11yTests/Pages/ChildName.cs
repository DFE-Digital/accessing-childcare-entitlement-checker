using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildNamePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildNamePageHasNoAccessibilityViolations()
    {
        await StartJourney();
        await AnswerLocation();
        await ExpectPathAndQuery("/children/add-child-details");
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildNamePageWithValidationErrorHasNoAccessibilityViolations()
    {
        await StartJourney();
        await AnswerLocation();
        await ExpectPathAndQuery("/children/add-child-details");
        await Continue();
        await ExpectPathAndQuery("/children/add-child-details");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}