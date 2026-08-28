using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildIsBornPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task HasChildBeenBornPageHasNoAccessibilityViolations()
    {
        await GoToHasChildBeenBornPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task HasChildBeenBornPageWithValidationErrorHasNoAccessibilityViolations()
    {
        var childId = await GoToHasChildBeenBornPage();
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}