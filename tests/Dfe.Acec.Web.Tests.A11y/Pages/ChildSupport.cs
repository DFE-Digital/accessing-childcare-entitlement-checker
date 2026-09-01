using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class ChildSupportPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildSupportPageHasNoAccessibilityViolations()
    {
        var childId = await GoToChildDateOfBirthPage();
        await EnterChildDateOfBirth(childId);
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildSupportPageWithValidationErrorHasNoAccessibilityViolations()
    {
        var childId = await GoToChildDateOfBirthPage();
        await EnterChildDateOfBirth(childId);
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}