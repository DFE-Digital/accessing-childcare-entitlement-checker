namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ResultsDetailedPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsDetailedPage_HasNoAccessibilityViolations()
    {
        var childId = await CompleteJourneyToResultsDetailed();
        await ExpectPathAndQuery($"/Results/ResultsDetailed?childId={childId}");
        await EvaluatePage();
    }
}