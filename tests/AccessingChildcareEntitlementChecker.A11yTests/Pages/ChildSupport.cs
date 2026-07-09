using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildSupportPage_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId);
        await EnterChildDateOfBirth(childId);
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildSupportPage_WithValidationError_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId);
        await EnterChildDateOfBirth(childId);
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}