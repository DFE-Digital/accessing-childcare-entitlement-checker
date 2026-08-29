namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class ResultsPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task ResultsPageHasNoAccessibilityViolations()
    {
        await CompleteJourneyToResults();
        await ExpectPathAndQuery("/results");
        await EvaluatePage();
    }
}