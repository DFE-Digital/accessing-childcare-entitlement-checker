using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Tests.A11y.Pages;

public class ChildDueDatePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WhatIsChildsDueDatePageHasNoAccessibilityViolations()
    {
        await GoToExpectedChildDueDatePage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WhatIsChildsDueDatePageWithValidationErrorHasNoAccessibilityViolations()
    {
        var childId = await GoToExpectedChildDueDatePage();
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/expectant-childs-due-date");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}