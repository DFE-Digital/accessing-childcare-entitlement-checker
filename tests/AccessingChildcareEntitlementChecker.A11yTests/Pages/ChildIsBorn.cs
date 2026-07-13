using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;


public class ChildIsBornPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{

    [Fact]
    public async Task HasChildBeenBornPage_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");
        await EvaluatePage();
    }

    [Fact]
    public async Task HasChildBeenBornPage_WithValidationError_HasNoAccessibilityViolations()
    {
        var childId = await AddChild();
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}