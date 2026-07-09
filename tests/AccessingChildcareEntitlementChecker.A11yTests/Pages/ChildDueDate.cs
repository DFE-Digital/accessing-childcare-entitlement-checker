using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildDueDatePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WhatIsChildsDueDatePage_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId, false);
        await Continue();
        await EvaluatePage();
    }

    [Fact]
    public async Task WhatIsChildsDueDatePage_WithValidationError_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId, false);
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}