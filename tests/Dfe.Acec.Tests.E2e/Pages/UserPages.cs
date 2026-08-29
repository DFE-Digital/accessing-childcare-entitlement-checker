using Microsoft.Playwright;

namespace Dfe.Acec.Tests.E2e.Pages;

[PagePattern(PageNames.StartPage)]
internal sealed class StartPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.StartPage;

    public override Task AnswerAsync(string answer) => Task.CompletedTask; // No question to answer

    public override async Task ContinueAsync()
    {
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();
    }
}

[PagePattern(PageNames.Location)]
internal sealed class LocationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Location;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UserAge)]
internal sealed class UserAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.Nationality)]
internal sealed class NationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Nationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PaidWork)]
internal sealed class PaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.ParentalLeave)]
internal sealed class ParentalLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ParentalLeave;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.WorkStatus)]
internal sealed class WorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.WorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.SelfEmployedDuration)]
internal sealed class SelfEmployedDurationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.SelfEmployedDuration;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.WeeklyEarnings)]
internal sealed class WeeklyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.WeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.YearlyEarnings)]
internal sealed class YearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.YearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UniversalCredit)]
internal sealed class UniversalCreditPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UniversalCredit;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.Benefits)]
internal sealed class BenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Benefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.ChildcareSupport)]
internal sealed class ChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.ChildcareVoucherReceipt)]
internal sealed class ChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UserSettledStatus)]
internal sealed class UserSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.TypeOfLeave)]
internal sealed class TypeOfLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.TypeOfLeave;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
