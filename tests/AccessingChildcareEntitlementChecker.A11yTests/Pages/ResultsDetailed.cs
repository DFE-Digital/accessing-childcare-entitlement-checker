namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ResultsDetailedPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsDetailedPage_HasNoAccessibilityViolations()
    {
        await CompleteJourneyToResultsDetailed();
        await EvaluatePage();
    }
}