using Microsoft.Playwright;
using AccessingChildcareEntitlementChecker.E2eTests.Helpers;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal abstract class BasePage(IPage page) : IPageObject
{
    protected readonly IPage Page = page;

    public abstract string PageTitle { get; }

    public abstract Task AnswerAsync(string answer);

    public virtual async Task ContinueAsync()
    {
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();
    }

    protected virtual async Task AssertHeaderAsync()
    {
        var heading = Page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Level = 1 });
        await Expect(heading).ToContainTextAsync(PageTitle);
    }

    protected async Task SelectRadioAsync(string option)
    {
        await Page.GetByRole(AriaRole.Radio, new PageGetByRoleOptions { Name = option, Exact = true }).CheckAsync();
    }

    protected async Task FillTextAsync(string text)
    {
        var textboxes = Page.GetByRole(AriaRole.Textbox);
        await textboxes.First.FillAsync(text);
    }

    protected async Task FillDateAsync(string dateText)
    {
        var date = RelativeDate.Parse(dateText);
        await Page.GetByLabel("Day").FillAsync(date.Day.ToString());
        await Page.GetByLabel("Month").FillAsync(date.Month.ToString());
        await Page.GetByLabel("Year").FillAsync(date.Year.ToString());
    }

    protected async Task CheckCheckboxesAsync(string checkboxAnswers)
    {
        foreach (var checkboxAnswer in checkboxAnswers.Split(';').Select(a => a.Trim()))
        {
            await Page.GetByRole(AriaRole.Checkbox, new PageGetByRoleOptions { Name = checkboxAnswer, Exact = true }).CheckAsync();
        }
    }
}
