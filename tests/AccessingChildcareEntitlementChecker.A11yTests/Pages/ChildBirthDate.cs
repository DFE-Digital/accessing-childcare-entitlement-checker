using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;


public class ChildBirthDatePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WhatIsChildsBirthDatePage_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId);
        await Continue();
        await EvaluatePage();
    }

    [Fact]
    public async Task WhatIsChildsBirthDatePage_WithValidationError_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await AnswerChildHasBeenBorn(childId);
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}