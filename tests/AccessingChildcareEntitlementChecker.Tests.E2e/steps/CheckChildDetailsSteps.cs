using Microsoft.Playwright;
using Reqnroll;
using System.Globalization;
using static Microsoft.Playwright.Assertions;
using static System.Net.WebRequestMethods;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class CheckChildDetailsSteps
    {
        private Context _context;

        public CheckChildDetailsSteps(Context context)
        {
            _context = context;
        }

        [Then("I should see one summary panel")]
        public async Task ThenIShouldSeeOneSummaryPanel()
        {
            await Expect(_context.Page.Locator(".govuk-summary-card")).ToHaveCountAsync(1);
        }

        [Then("I should see {int} summary panels")]
        public async Task ThenIShouldSeeSummaryPanels(int expectedSummaryPanelCount)
        {
            await Expect(_context.Page.Locator(".govuk-summary-card")).ToHaveCountAsync(expectedSummaryPanelCount);
        }

        [Then("I should see a summary panel with the title {string} and the following summary:")]
        public async Task ThenIShouldSeeASummaryPanelWithTheTitleAndTheFollowingSummary(string title, DataTable dataTable)
        {
            var panel = _context.Page.Locator(".govuk-summary-card")
                    .Filter(new() { HasTextString = title });

            await Expect(panel).ToBeVisibleAsync();
            await Expect(panel.Locator(".govuk-summary-card__title")).ToHaveTextAsync(title);
            foreach (var row in dataTable.Rows)
            {
                var question = row["Question"];
                var answer = row["Answer"];

                var summaryRow = panel.Locator(".govuk-summary-list__row")
                    .Filter(new() { HasTextString = question });

                await Expect(summaryRow).ToBeVisibleAsync();

                await Expect(summaryRow.Locator(".govuk-summary-list__key"))
                    .ToHaveTextAsync(question);

                if (answer.ToLower() == "tomorrow" || answer.ToLower() == "yesterday" || answer.ToLower() == "today")
                {
                    var expectedDate = ParseRelativeDate(answer).ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("en-GB"));
                    await Expect(summaryRow.Locator(".govuk-summary-list__value"))
                        .ToContainTextAsync(expectedDate);
                }
                else
                {
                    await Expect(summaryRow.Locator(".govuk-summary-list__value"))
                        .ToContainTextAsync(answer);
                }
            }
        }

        [When("I click the Change link in the {string} panel for {string}")]
        public async Task WhenIClickTheChangeLinkInThePanelFor(string title, string question)
        {
            var panel = _context.Page.Locator(".govuk-summary-card")
                    .Filter(new() { HasTextString = title });

            var summaryRow = panel.Locator(".govuk-summary-list__row")
                .Filter(new() { HasTextString = question });

            await summaryRow.GetByRole(AriaRole.Link, new() { Name = "Change" }).ClickAsync();
        }

        [When("I click the Delete link in the {string} panel")]
        public async Task WhenIClickTheDeleteLinkInThePanel(string title)
        {
            var panel = _context.Page.Locator(".govuk-summary-card")
                    .Filter(new() { HasTextString = title });

            await panel.GetByRole(AriaRole.Link, new() { Name = "Delete" }).ClickAsync();
        }

        // @TODO DON'T COPYPASTA
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
