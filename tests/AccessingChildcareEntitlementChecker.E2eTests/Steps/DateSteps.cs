using Reqnroll;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class DateSteps(IPage page)
{
    [When("I enter the day {string} month {string} and year {string}")]
    public async Task WhenIEnterTheDayStringMonthStringAndYearString(string day, string month, string year)
    {
        await EnterDate(day, month, year);
    }

    [When("I enter tomorrow's date")]
    public async Task WhenIEnterTomorrowsDate()
    {
        await EnterDate(DateTime.UtcNow.AddDays(1));
    }

    [When("I do not enter a date")]
    public async Task WhenIDoNotEnterADate()
    {
        await EnterDate(string.Empty, string.Empty, string.Empty);
    }

    [When("I enter yesterdays date")]
    public async Task WhenIEnterYesterdaysDate()
    {
        await EnterDate(DateTime.UtcNow.AddDays(-1));
    }

    [When("I enter todays date")]
    public async Task WhenIEnterTodaysDate()
    {
        await EnterDate(DateTime.UtcNow);
    }

    [Then("I should see a date entry input")]
    public async Task ThenIShouldSeeADateEntryInput()
    {
        await Expect(page.GetByLabel("Day"))
            .ToBeVisibleAsync();
        await Expect(page.GetByLabel("Month"))
            .ToBeVisibleAsync();
        await Expect(page.GetByLabel("Year"))
            .ToBeVisibleAsync();
    }

    [Then("the date entry input is empty")]
    public async Task ThenTheDateEntryInputIsEmpty()
    {
        await Expect(page.GetByLabel("Day"))
            .ToHaveValueAsync(string.Empty);
        await Expect(page.GetByLabel("Month"))
            .ToHaveValueAsync(string.Empty);
        await Expect(page.GetByLabel("Year"))
            .ToHaveValueAsync(string.Empty);
    }

    private async Task EnterDate(DateTime date)
    {
        await EnterDate(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());
    }

    private async Task EnterDate(string day, string month, string year)
    {
        await page.GetByLabel("Day").FillAsync(day);
        await page.GetByLabel("Month").FillAsync(month);
        await page.GetByLabel("Year").FillAsync(year);
    }
}
