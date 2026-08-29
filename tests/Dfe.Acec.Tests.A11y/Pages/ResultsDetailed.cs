namespace Dfe.Acec.Tests.A11y.Pages;

public class ResultsDetailedPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsDetailedPageHasNoAccessibilityViolations()
    {
        await CompleteJourneyToResultsDetailed();
        await EvaluatePage();
    }
}