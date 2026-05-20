using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.ChildDetails;

[Binding]
public class DateSteps
{
    private readonly Context _context;

    public DateSteps(Context context)
    {
        _context = context;
    }

    [When("I enter the day {string} month {string} and year {string}")]
    public async Task GivenIEnterTheDayMonthAndYear(string day, string month, string year)
    {
        await EnterDate(day, month, year);
    }

    [When("I enter tomorrow's date")]
    public async Task GivenIEnterTomorrowsDate()
    {
        await EnterDate(DateTime.UtcNow.AddDays(1));
    }

    [When("I do not enter a date")]
    public async Task GivenIHaveNotEnteredADate()
    {
        await EnterDate(string.Empty, string.Empty, string.Empty);
    }

    [When("I enter yesterdays date")]
    public async Task GivenIHaveEnteredYesterdaysDate()
    {
        await EnterDate(DateTime.UtcNow.AddDays(-1));
    }

    [When("I enter todays date")]
    public async Task GivenIHaveEnteredTodaysDate()
    {
        await EnterDate(DateTime.UtcNow);
    }

    [Then("I should see a date entry input")]
    public async Task ThenIShouldSeeADateEntryInput()
    {
        await Expect(_context.Page.GetByLabel("Day"))
            .ToBeVisibleAsync();
        await Expect(_context.Page.GetByLabel("Month"))
            .ToBeVisibleAsync();
        await Expect(_context.Page.GetByLabel("Year"))
            .ToBeVisibleAsync();
    }

    private async Task EnterDate(DateTime date)
    {
        await EnterDate(date.Day.ToString(), date.Month.ToString(), date.Year.ToString());
    }

    private async Task EnterDate(string day, string month, string year)
    {
        await _context.Page.GetByLabel("Day").FillAsync(day);
        await _context.Page.GetByLabel("Month").FillAsync(month);
        await _context.Page.GetByLabel("Year").FillAsync(year);
    }
}
