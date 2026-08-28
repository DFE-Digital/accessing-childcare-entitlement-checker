namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ChildSummaryPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ChildSummaryPageHasNoAccessibilityViolations()
    {
        var childId = await CompleteBornChildToSummary();
        await ExpectPathAndQuery($"/children/check-childs-details?childId={childId}");
        await EvaluatePage();
    }
}