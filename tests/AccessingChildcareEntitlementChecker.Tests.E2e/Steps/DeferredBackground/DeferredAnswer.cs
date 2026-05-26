using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground;

public class DeferredAnswer(string title, string answer, string scope) : IDeferredAction
{
    private string _answer = answer;

    public string Title { get; } = title;

    public string Scope { get; } = scope;

    public DeferredAnswer(string title, string answer) : this(title, answer, string.Empty)
    {

    }

    public async Task Execute(IPage page)
    {
        var heading = page.GetByRole(AriaRole.Heading, new() { Level = 1 });
        await Expect(heading).ToHaveTextAsync(Title);
        await AnswerQuestion(page);
        await page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
        await Expect(heading).Not.ToHaveTextAsync(Title);
    }

    private async Task AnswerQuestion(IPage page)
    {
        var dateInputs = page.Locator(".govuk-date-input input");

        if (await dateInputs.CountAsync() > 0)
        {
            var date = RelativeDate.Parse(_answer);

            await page.GetByLabel("Day").FillAsync(date.Day.ToString());
            await page.GetByLabel("Month").FillAsync(date.Month.ToString());
            await page.GetByLabel("Year").FillAsync(date.Year.ToString());
            return;
        }

        var textboxes = page.GetByRole(AriaRole.Textbox);
        if (await textboxes.CountAsync() > 0)
        {
            await textboxes.First.FillAsync(_answer);
            return;
        }

        var radioButtons = page.GetByRole(AriaRole.Radio);
        if (await radioButtons.CountAsync() > 0)
        {
            await page.GetByRole(AriaRole.Radio, new() { Name = _answer, Exact = true }).CheckAsync();
            return;
        }

        foreach (var checkboxAnswer in _answer.Split(';').Select(a => a.Trim()))
        {
            await page.GetByRole(AriaRole.Checkbox, new() { Name = checkboxAnswer, Exact = true }).CheckAsync();
        }
    }
}
