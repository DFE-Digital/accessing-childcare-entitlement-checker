using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

[PagePattern(PageNames.ChildName)]
internal class ChildNamePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildName;
    public override async Task AnswerAsync(string answer) => await FillTextAsync(answer);
}

[PagePattern(PageNames.ChildIsBorn)]
internal class ChildIsBornPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildIsBorn;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.ChildDueDate)]
internal class ChildDueDatePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildDueDate;
    public override async Task AnswerAsync(string answer) => await FillDateAsync(answer);
}

[PagePattern(PageNames.ChildBirthDate)]
internal class ChildBirthDatePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildBirthDate;
    public override async Task AnswerAsync(string answer) => await FillDateAsync(answer);
}

[PagePattern(PageNames.ChildSupport)]
internal class ChildSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}
