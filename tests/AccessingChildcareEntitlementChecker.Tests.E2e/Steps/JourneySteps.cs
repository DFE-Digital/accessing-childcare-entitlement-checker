using AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground;
using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

/// <summary>
/// Steps relating to navigating through the journey.
/// </summary>
/// <remarks>
/// Note that steps along the journey are queued up as deferred actions that are
/// executed the first time a "When" or "Then" block is encountered; see `DeferredActionHooks.cs`.
///
/// This allows for building the journey in the feature `Background`, and later making small
/// modifications to past answers so that small variations of behaviour due to past answers
/// can be easily tested without requiring a feature file per journey variation.
///
/// For an example of this see the tests for paid work.
/// </remarks>
/// <param name="context">Context of the currently running test.</param>
[Binding]
public class JourneySteps(Context context)
{
    private Context _context = context;

    [Given("I am on the childcare entitlement checker website")]
    public async Task GivenIAmOnTheChildcareEntitlementCheckerWebsite()
    {
        await _context.Page.GotoAsync(_context.Uri.ToString());
    }

    [Given("I click the Start now link")]
    public void GivenIClickTheStartNowLink()
    {
        _context.Queue.ClickLink("Check what help you could get with childcare costs", "Start Now");
    }

    [Given("I answer {string} as {string}")]
    public void GivenIAnswerAs(string question, string answer)
    {
        _context.Queue.Answer(question, answer);
    }

    [Given("I answer questions for {string} as follows:")]
    public void GivenIAnswerQuestionsForWithTheFollowingAnswers(string scope, DataTable answers)
    {
        _context.Queue.ScopedAnswers(scope, answers.ToQuestionAnswerPairs());
    }

    [Given("I answer questions as follows:")]
    public void GivenIAnswerQuestionsAsFollows(DataTable answers)
    {
        _context.Queue.Answers(answers.ToQuestionAnswerPairs());
    }

    [Given("I start the journey, filling in Aydin's and Sara's details")]
    public void GivenIStartTheJourneyFillingInAydinsAndSarasDetails()
    {
        GivenIClickTheStartNowLink();
        _context.Queue.Answer("Where do you live?", "England");
        _context.Queue.ScopedAnswers("Aydin", [
            ("Add details of a child", "Aydin"),
            ("Has this child been born yet?", "No"),
            ("What is this child's due date?", "Tomorrow"),
            ("What will your relationship be to this child?", "Parent"),
        ]);
        _context.Queue.ClickButton("Check your children's details", "Add another child");
        _context.Queue.ScopedAnswers("Sara", [
            ("Add details of a child", "Sara"),
            ("Has this child been born yet?", "Yes"),
            ("What is Sara's date of birth?", "Yesterday"),
            ("What is your relationship to Sara?", "Parent"),
            ("Does Sara get any of the following support?", "Education, health and care (EHC) plan"),
        ]);
    }

    [Given("I check my children's details and click on Continue")]
    public void GivenICheckMyChildrensDetailsAndClickOnContinue()
    {
        _context.Queue.ClickButton("Check your children's details", "Continue");
    }

    [Given("I fill in my own details")]
    public void GivenIFillInMyOwnDetails()
    {
        _context.Queue.Answers([
            ("What is your age?", "Under 18"),
            ("What is your nationality?", "British or Irish citizen"),
            ("Are you in paid work?", "No"),
            ("Does your household receive universal credit?", "Yes"),
            ("Do you get any of these benefits?", "Carer's Allowance"),
            ("Do you already get any of this childcare support?", "Childcare vouchers"),
            ("How do you receive your childcare vouchers?", "A workplace nursery scheme"),
        ]);
    }

    [Given("I click the Add another child button")]
    public void GivenIClickTheAddAnotherChildButton()
    {
        _context.Queue.ClickButton("Check your children's details", "Add another child");
    }

    /// <summary>
    /// Answer further questions after the `Given` clauses.
    /// </summary>
    /// <remarks>
    /// Since we're past the `Given` clauses; we've already executed the background,
    /// so these steps can no longer be deferred and must be executed immediately.
    /// </remarks>
    /// <param name="answers">The list of questions and answers.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    [When("I answer the questions as follows:")]
    public async Task WhenIAnswerTheQuestionsAsFollows(DataTable answers)
    {
        var actions = answers
            .ToQuestionAnswerPairs()
            .Select(pair => new DeferredAnswer(pair.Question, pair.Answer));
        foreach (var action in actions)
        {
            await action.Execute(_context.Page);
        }
    }

    [When("the page header is {string}")]
    public async Task WhenThePageHeaderIs(string expectedHeader)
    {
        await _context.Page
            .GetByRole(AriaRole.Heading, new() { Name = expectedHeader })
            .WaitForAsync();
    }

    [When("I click on Continue")]
    public async Task WhenIClickOnContinue()
    {
        await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
    }

    [When("I click the back link")]
    public async Task WhenIClickTheBackLink()
    {
        await _context.Page
            .Locator(".govuk-back-link")
            .ClickAsync();
    }

    [Then("the page header is {string}")]
    public async Task ThenThePageHeaderIs(string expectedHeader)
    {
        await Expect(_context.Page
            .GetByRole(AriaRole.Heading, new() { Level = 1 })
        ).ToHaveTextAsync(expectedHeader);
    }

    [Then("I will be directed to the next page in the user journey {string}")]
    public async Task ThenIWillBeDirectedToTheNextPageInTheUserJourney(string expectedHeader)
    {
        await Expect(_context.Page.Locator("body"))
            .ToContainTextAsync(expectedHeader);
    }

    [Then("I should be returned to the previous page in the user journey {string}")]
    public async Task ThenIShouldBeReturnedToThePreviousPageInTheUserJourney(string expectedHeader)
    {
        await Expect(
            _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
        ).ToHaveTextAsync(expectedHeader);
    }
}
