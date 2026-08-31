using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class ChildBirthDatePageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task WhatIsChildsBirthDatePageHasNoAccessibilityViolations()
    {
        await GoToChildDateOfBirthPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task WhatIsChildsBirthDatePageWithValidationErrorHasNoAccessibilityViolations()
    {
        var childId = await GoToChildDateOfBirthPage();
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/childs-date-of-birth");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
