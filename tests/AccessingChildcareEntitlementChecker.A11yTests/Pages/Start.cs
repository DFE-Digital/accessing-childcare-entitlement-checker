namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class StartPageAccessibilityTests(ITestOutputHelper output) : PageBase(output)
{
    protected override string PageUrl => "/";

    [Fact]
    public async Task StartPageHasNoAccessibilityViolations()
    {
        await GoToPage();
        await EvaluatePage();
    }
}