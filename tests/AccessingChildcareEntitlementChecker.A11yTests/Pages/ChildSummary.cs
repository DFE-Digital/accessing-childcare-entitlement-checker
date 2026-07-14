using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildSummaryPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildSummaryPage_HasNoAccessibilityViolations()
    {
        var childId = await CompleteBornChildToSummary();
        await ExpectPathAndQuery($@"/children/check-childs-details?childId={childId}");
        await EvaluatePage();
    }
}