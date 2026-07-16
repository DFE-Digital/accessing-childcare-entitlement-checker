namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class ResultsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsPage_HasNoAccessibilityViolations()
    {
        await CompleteJourneyToResults();
        await ExpectPathAndQuery("/results");
        await EvaluatePage();
    }
}