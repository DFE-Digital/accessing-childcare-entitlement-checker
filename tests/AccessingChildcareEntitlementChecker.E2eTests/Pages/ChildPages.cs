using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal class ChildNamePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildName;
    public override async Task AnswerAsync(string answer) => await FillTextAsync(answer);
}

internal class ChildIsBornPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildIsBorn;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class ChildDueDatePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildDueDate;
    public override async Task AnswerAsync(string answer) => await FillDateAsync(answer);
}

internal class ChildBirthDatePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildBirthDate;
    public override async Task AnswerAsync(string answer) => await FillDateAsync(answer);
}

internal class ChildRelationshipPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildRelationship;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class ChildSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}
