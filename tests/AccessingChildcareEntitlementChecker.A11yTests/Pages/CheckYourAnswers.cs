namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class CheckYourAnswersPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task CheckYourAnswersPageHasNoAccessibilityViolations()
    {
        await CompleteJourneyToCheckYourAnswers();
        await ExpectPathAndQuery("/check-your-answers");
        await EvaluatePage();
    }
}