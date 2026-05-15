using Microsoft.Playwright;
using Reqnroll;
using System.Globalization;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class JourneySteps
    {
        private Context _context;

        public JourneySteps(Context context)
        {
            _context = context;
        }

        [Given("I start the journey and answer the questions as follows:")]
        public async Task GivenIStartTheJourneyAndAnswerTheQuestionsAsFollows(DataTable dataTable)
        {
            await _context.Page
               .GetByRole(AriaRole.Link, new() { Name = "Start now" })
               .ClickAsync();

            foreach (var step in dataTable.Rows)
            {
                var question = step[0];
                var answer = step[1];
                var heading = _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 });
                await AssertHeader(question);
                await AnswerQuestion(answer);

                await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
                await Expect(heading).Not.ToHaveTextAsync(question);
            }
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
                var date = ParseRelativeDate(answer);

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

            await _context.Page.GetByRole(AriaRole.Radio, new() { Name = answer, Exact = true }).CheckAsync();
        }

        /// <remarks>
        /// Consider using a lib like Humanizer if this grows any bigger.
        /// </remarks>
        private DateOnly ParseRelativeDate(string value)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            return value.Trim().ToLowerInvariant() switch
            {
                "today" => today,
                "yesterday" => today.AddDays(-1),
                "tomorrow" => today.AddDays(1),
                _ => DateOnly.Parse(value, CultureInfo.GetCultureInfo("en-GB"))
            };
        }
    }
}
