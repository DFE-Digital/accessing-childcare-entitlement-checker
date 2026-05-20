using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps;

[Binding]
public class JourneySteps
{
    private Context _context;

    public JourneySteps(Context context)
    {
        _context = context;
    }

    [Given("I start the journey, filling in Aydin's and Sara's details")]
    public async Task GivenIStartTheJourneyFillingInAydinsAndSarasDetails()
    {
        await _context.Page
           .GetByRole(AriaRole.Link, new() { Name = "Start now" })
           .ClickAsync();

        await Respond(new Response("Where do you live?", "England"));

        await Respond([
            new ("Add details of a child", "Aydin"),
            new ("Has this child been born yet?", "No"),
            new ("What is this child's due date?", "Tomorrow"),
            new ("What will your relationship be to this child?", "Parent"),
            ]);

        await _context.Page
            .GetByRole(AriaRole.Button, new() { Name = "Add another child" })
            .ClickAsync();

        await Respond([
            new ("Add details of a child", "Sara"),
            new ("Has this child been born yet?", "Yes"),
            new ("What is Sara's date of birth?", "Yesterday"),
            new ("What is your relationship to Sara?", "Parent"),
            new ("Does Sara get any of the following support?", "Education, health and care (EHC) plan"),
            ]);
    }

    [Given("I start the journey and answer the questions as follows:")]
    public async Task GivenIStartTheJourneyAndAnswerTheQuestionsAsFollows(DataTable dataTable)
    {
        await _context.Page
           .GetByRole(AriaRole.Link, new() { Name = "Start now" })
           .ClickAsync();

        await Respond(dataTable);
    }

    [Given("I click the Add another child button and answer the questions as follows:")]
    public async Task GivenIClickTheAddAnotherChildButtonAndAnswerTheQuestionsAsFollows(DataTable dataTable)
    {
        await _context.Page
            .GetByRole(AriaRole.Button, new() { Name = "Add another child" })
            .ClickAsync();

        await Respond(dataTable);
    }

    [Given("I answer the questions as follows:")]
    [When("I answer the questions as follows:")]
    public async Task WhenIAnswerTheQuestionsAsFollows(DataTable dataTable)
    {
        await Respond(dataTable);
    }

    private async Task Respond(DataTable dataTable)
    {
        var responses = dataTable.Rows.Select(row => new Response(row["Question"], row["Answer"]));
        await Respond(responses);
    }

    private async Task Respond(IEnumerable<Response> responses)
    {
        foreach (var response in responses)
        {
            await Respond(response);
        }
    }

    private async Task Respond(Response response)
    {
        var question = response.Question;
        var answer = response.Answer;
        var heading = _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 });
        await AssertHeader(question);
        await AnswerQuestion(answer);

        await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
        await Expect(heading).Not.ToHaveTextAsync(question);
    }

    private async Task AssertHeader(string expectedHeader)
    {
        await Expect(
            _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
        ).ToHaveTextAsync(expectedHeader);
    }

    private async Task AnswerQuestion(string answer)
    {
        var dateInputs = _context.Page.Locator(".govuk-date-input input");

        if (await dateInputs.CountAsync() > 0)
        {
            var date = RelativeDate.Parse(answer);

            await _context.Page.GetByLabel("Day").FillAsync(date.Day.ToString());
            await _context.Page.GetByLabel("Month").FillAsync(date.Month.ToString());
            await _context.Page.GetByLabel("Year").FillAsync(date.Year.ToString());
            return;
        }

        var textboxes = _context.Page.GetByRole(AriaRole.Textbox);
        if (await textboxes.CountAsync() > 0)
        {
            await textboxes.First.FillAsync(answer);
            return;
        }

        var radioButtons = _context.Page.GetByRole(AriaRole.Radio);
        if (await radioButtons.CountAsync() > 0)
        {
            await _context.Page.GetByRole(AriaRole.Radio, new() { Name = answer, Exact = true }).CheckAsync();
            return;
        }

        foreach (var checkboxAnswer in answer.Split(';').Select(a => a.Trim()))
        {
            await _context.Page.GetByRole(AriaRole.Checkbox, new() { Name = checkboxAnswer, Exact = true }).CheckAsync();
        }
    }

    private record Response(string Question, string Answer);
}
