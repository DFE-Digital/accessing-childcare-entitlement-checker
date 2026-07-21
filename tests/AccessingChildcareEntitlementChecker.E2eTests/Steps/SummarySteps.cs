using Microsoft.Playwright;
using Reqnroll;
using System.Globalization;
using System.Text.RegularExpressions;
using AccessingChildcareEntitlementChecker.E2eTests.Helpers;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class SummarySteps(IPage page)
{
    [Given("I do not see a summary list for {string}")]
    [Then("I do not see a summary list for {string}")]
    public async Task ThenIDoNotSeeASummaryListForString(string title)
    {
        var heading = page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions
        {
            Name = title,
            Level = 2
        });

        await Expect(heading).Not.ToBeVisibleAsync();
    }

    [When("I click the Change link in the {string} card for {string}")]
    public async Task WhenIClickTheChangeLinkInTheStringCardForString(string title, string question)
    {
        var panel = page.Locator(".govuk-summary-card")
                .Filter(new LocatorFilterOptions { HasTextString = title });

        var summaryRow = panel.Locator(".govuk-summary-list__row")
            .Filter(new LocatorFilterOptions { HasTextString = question });

        await summaryRow.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Change" }).ClickAsync();
    }

    [When("I click the Remove link in the {string} card")]
    public async Task WhenIClickTheRemoveLinkInTheStringCard(string title)
    {
        var panel = page.Locator(".govuk-summary-card")
                .Filter(new LocatorFilterOptions { HasTextString = title });

        await panel.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Remove" }).ClickAsync();
    }

    [Given("I remove {string}")]
    [When("I remove {string}")]
    public async Task WhenIRemoveString(string name)
    {
        var panel = page.Locator(".govuk-summary-card")
                .Filter(new LocatorFilterOptions { HasTextString = name });

        await panel.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Remove" }).ClickAsync();
        await Expect(page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Name = $"Are you sure you want to remove {name}?" }))
            .ToBeVisibleAsync();
        await page.GetByRole(AriaRole.Radio, new PageGetByRoleOptions { Name = "Yes" }).CheckAsync();
        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();
    }

    [When("I click the Change link in the {string} summary list for {string}")]
    public async Task WhenIClickTheChangeLinkInTheStringSummaryListForString(string title, string question)
    {
        var heading = page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions
        {
            Name = title,
            Level = 2
        });

        var summaryList = heading.Locator(
            "xpath=following-sibling::*[contains(@class,'govuk-summary-list')][1]"
        );

        var summaryRow = summaryList.Locator(".govuk-summary-list__row")
            .Filter(new LocatorFilterOptions { HasTextString = question });

        await summaryRow.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = "Change" }).ClickAsync();
    }

    [Then("I should see one summary card")]
    public async Task ThenIShouldSeeOneSummaryCard()
    {
        await Expect(page.Locator(".govuk-summary-card")).ToHaveCountAsync(1);
    }

    [Then("I should see {int} summary cards")]
    public async Task ThenIShouldSeeIntSummaryCards(int expectedSummaryCardCount)
    {
        await Expect(page.Locator(".govuk-summary-card")).ToHaveCountAsync(expectedSummaryCardCount);
    }

    [Then("I should see a summary card with the title {string} and the following summary:")]
    public async Task ThenIShouldSeeASummaryCardWithTheTitleStringAndTheFollowingSummary(string title, DataTable dataTable)
    {
        var panel = page.Locator(".govuk-summary-card")
                .Filter(new LocatorFilterOptions { HasTextString = title });

        await Expect(panel).ToBeVisibleAsync();
        await Expect(panel.Locator(".govuk-summary-card__title")).ToHaveTextAsync(title);
        foreach (var row in dataTable.Rows)
        {
            var question = row["Question"];
            var answer = row["Answer"];

            var summaryRow = panel.Locator(".govuk-summary-list__row")
                .Filter(new LocatorFilterOptions { HasTextString = question });

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
    public async Task ThenIShouldSeeSomeTextSayingString(string expectedText)
    {
        await Expect(page.Locator("body")).ToContainTextAsync(expectedText);
    }

    [Then("I should not see a Continue button")]
    public async Task ThenIShouldNotSeeAContinueButton()
    {
        await Expect(page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }))
            .Not.ToBeVisibleAsync();
    }

    [Then("I should see a summary list for {string} with the following summary:")]
    public async Task ThenIShouldSeeASummaryListForStringWithTheFollowingSummary(string title, DataTable dataTable)
    {
        var heading = page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions
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

            ILocator summaryRow;
            if (question.Contains("__PLACEHOLDER__"))
            {
                var regexPattern = Regex.Escape(question).Replace("__PLACEHOLDER__", "(.*?)");
                var regex = new Regex(regexPattern);
                summaryRow = summaryList.Locator(".govuk-summary-list__row")
                    .Filter(new LocatorFilterOptions { HasTextRegex = regex });

                await Expect(summaryRow).ToBeVisibleAsync();
                await Expect(summaryRow.Locator(".govuk-summary-list__key")).ToHaveTextAsync(regex);
            }
            else
            {
                summaryRow = summaryList.Locator(".govuk-summary-list__row")
                    .Filter(new LocatorFilterOptions { HasTextString = question });

                await Expect(summaryRow).ToBeVisibleAsync();
                await Expect(summaryRow.Locator(".govuk-summary-list__key")).ToHaveTextAsync(question);
            }

            await Expect(summaryRow.Locator(".govuk-summary-list__value"))
                .ToHaveTextAsync(answer);
        }
    }

    [Then("I do not see a summary row {string}")]
    public async Task ThenIDoNotSeeASummaryRowString(string question)
    {
        var summaryRow = page.Locator(".govuk-summary-list__row")
                .Filter(new LocatorFilterOptions { HasTextString = question });

        await Expect(summaryRow).Not.ToBeVisibleAsync();
    }
}
