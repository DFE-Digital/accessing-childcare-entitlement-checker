using Microsoft.Playwright;
using Reqnroll;
using System.Globalization;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class SummarySteps
{
    private Context _context;

    public SummarySteps(Context context)
    {
        _context = context;
    }

    [Given("I do not see a summary list for {string}")]
    [Then("I do not see a summary list for {string}")]
    public void ThenIDoNotSeeASummaryListFor(string title)
    {
        var heading = _context.Page.GetByRole(AriaRole.Heading, new()
        {
            Name = title,
            Level = 2
        });

        Expect(heading).Not.ToBeVisibleAsync();
    }

    [When("I click the Change link in the {string} card for {string}")]
    public async Task WhenIClickTheChangeLinkInTheCardFor(string title, string question)
    {
        var panel = _context.Page.Locator(".govuk-summary-card")
                .Filter(new() { HasTextString = title });

        var summaryRow = panel.Locator(".govuk-summary-list__row")
            .Filter(new() { HasTextString = question });

        await summaryRow.GetByRole(AriaRole.Link, new() { Name = "Change" }).ClickAsync();
    }

    [When("I click the Remove link in the {string} card")]
    public async Task WhenIClickTheRemoveLinkInTheCard(string title)
    {
        var panel = _context.Page.Locator(".govuk-summary-card")
                .Filter(new() { HasTextString = title });

        await panel.GetByRole(AriaRole.Link, new() { Name = "Remove" }).ClickAsync();
    }

    [When("I remove {string}")]
    public async Task WhenIRemove(string name)
    {
        var panel = _context.Page.Locator(".govuk-summary-card")
                .Filter(new() { HasTextString = name });

        await panel.GetByRole(AriaRole.Link, new() { Name = "Remove" }).ClickAsync();
        await Expect(_context.Page.GetByRole(AriaRole.Heading, new() { Name = $"Are you sure you want to remove {name}?" }))
            .ToBeVisibleAsync();
        await _context.Page.GetByRole(AriaRole.Radio, new() { Name = "Yes" }).CheckAsync();
        await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
    }

    [When("I click the Change link in the {string} summary list for {string}")]
    public async Task WhenIClickTheChangeLinkInTheSummaryListFor(string title, string question)
    {
        var heading = _context.Page.GetByRole(AriaRole.Heading, new()
        {
            Name = title,
            Level = 2
        });

        var summaryList = heading.Locator(
            "xpath=following-sibling::*[contains(@class,'govuk-summary-list')][1]"
        );

        var summaryRow = summaryList.Locator(".govuk-summary-list__row")
            .Filter(new() { HasTextString = question });

        await summaryRow.GetByRole(AriaRole.Link, new() { Name = "Change" }).ClickAsync();
    }

    [Then("I should see one summary card")]
    public async Task ThenIShouldSeeOneSummaryCard()
    {
        await Expect(_context.Page.Locator(".govuk-summary-card")).ToHaveCountAsync(1);
    }

    [Then("I should see {int} summary cards")]
    public async Task ThenIShouldSeeSummaryCards(int expectedSummaryCardCount)
    {
        await Expect(_context.Page.Locator(".govuk-summary-card")).ToHaveCountAsync(expectedSummaryCardCount);
    }

    [Then("I should see a summary card with the title {string} and the following summary:")]
    public async Task ThenIShouldSeeASummaryCardWithTheTitleAndTheFollowingSummary(string title, DataTable dataTable)
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

            if (RelativeDate.IsRelative(answer))
            {
                var expectedDate = RelativeDate.Parse(answer).ToString("d MMMM yyyy", CultureInfo.GetCultureInfo("en-GB"));
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

    [Then("I should see some text saying {string}")]
    public async Task ThenIShouldSeeSomeTextSaying(string expectedText)
    {
        await Expect(_context.Page.Locator("body")).ToContainTextAsync(expectedText);
    }

    [Then("I should not see a Continue button")]
    public async Task ThenIShouldNotSeeAContinueButton()
    {
        await Expect(_context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }))
            .Not.ToBeVisibleAsync();
    }

    [Then("I should see a success banner that says {string}")]
    public async Task ThenIShouldSeeASuccessBannerThatSays(string p0)
    {
        var banner = _context.Page.Locator(".govuk-notification-banner--success");
        await Expect(banner).ToBeVisibleAsync();
        await Expect(banner.GetByRole(AriaRole.Heading, new() { Name = "Success" })).ToHaveTextAsync("Success");
        await Expect(banner.Locator(".govuk-notification-banner__content")).ToContainTextAsync(p0);
    }

    [Then("I should see a summary list for {string} with the following summary:")]
    public async Task ThenIShouldSeeASummaryListForWithTheFollowingSummary(string title, DataTable dataTable)
    {
        var heading = _context.Page.GetByRole(AriaRole.Heading, new()
        {
            Name = title,
            Level = 2
        });

        var summaryList = heading.Locator(
            "xpath=following-sibling::*[contains(@class,'govuk-summary-list')][1]"
        );

        foreach (var row in dataTable.Rows)
        {
            var question = row["Question"];
            var answer = row["Answer"];

            var summaryRow = summaryList.Locator(".govuk-summary-list__row")
                .Filter(new() { HasTextString = question });

            await Expect(summaryRow).ToBeVisibleAsync();

            await Expect(summaryRow.Locator(".govuk-summary-list__key"))
                .ToHaveTextAsync(question);

            await Expect(summaryRow.Locator(".govuk-summary-list__value"))
                .ToHaveTextAsync(answer);
        }
    }

    [Then("I do not see a summary row {string}")]
    public async Task ThenIDoNotSeeASummaryRow(string question)
    {
        var summaryRow = _context.Page.Locator(".govuk-summary-list__row")
                .Filter(new() { HasTextString = question });

        await Expect(summaryRow).Not.ToBeVisibleAsync();
    }
}
