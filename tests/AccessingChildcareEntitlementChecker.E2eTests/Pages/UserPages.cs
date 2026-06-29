using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal class StartPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.StartPage;

    public override Task AnswerAsync(string answer) => Task.CompletedTask; // No question to answer

    public override async Task ContinueAsync()
    {
        await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Start Now" }).ClickAsync();
    }
}

internal class LocationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Location;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class UserAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class NationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Nationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class WorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.WorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class SelfEmployedDurationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.SelfEmployedDuration;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class WeeklyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.WeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class YearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.YearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class UniversalCreditPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UniversalCredit;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class BenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Benefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class ChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class ChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class UserSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class TypeOfLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.TypeOfLeave;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
