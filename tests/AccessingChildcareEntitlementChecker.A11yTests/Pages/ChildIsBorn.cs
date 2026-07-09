using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;


public class ChildIsBornPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{

    [Fact]
    public async Task HasChildBeenBornPage_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId);
        await EvaluatePage();
    }

    [Fact]
    public async Task HasChildBeenBornPage_WithValidationError_HasNoAccessibilityViolations()
    {
        await AddChild();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}