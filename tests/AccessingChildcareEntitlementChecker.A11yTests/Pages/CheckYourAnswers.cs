namespace AccessingChildcareEntitlementChecker.A11yTests.Pages;

public class CheckYourAnswersPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task CheckYourAnswersPage_HasNoAccessibilityViolations()
    {
        await CompleteJourneyToCheckYourAnswers();
        await EvaluatePage();
    }
}