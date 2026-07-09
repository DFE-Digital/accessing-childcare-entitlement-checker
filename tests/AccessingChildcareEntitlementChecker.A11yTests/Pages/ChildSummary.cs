using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildSummaryPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildSummaryPage_HasNoAccessibilityViolations()
    {
        await CompleteBornChildToSummary();
        await EvaluatePage();
    }
}