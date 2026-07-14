using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;


namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildNamePageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/children/add-child-details";

    [Fact]
    public async Task ChildNamePage_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }

    [Fact]
    public async Task ChildNamePage_WithValidationError_HasNoAccessibilityViolations()
    {
        await GoToPage();
        await Continue();
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }


}