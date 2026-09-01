using JetBrains.Annotations;
using Microsoft.Playwright;

namespace Dfe.Acec.Web.Tests.E2e.Pages;

[PagePattern(PageNames.StartPage)]
[UsedImplicitly]
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
[UsedImplicitly]
internal sealed class LocationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Location;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UserAge)]
[UsedImplicitly]
internal sealed class UserAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.Nationality)]
[UsedImplicitly]
internal sealed class NationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Nationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PaidWork)]
[UsedImplicitly]
internal sealed class PaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.ParentalLeave)]
[UsedImplicitly]
internal sealed class ParentalLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ParentalLeave;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.WorkStatus)]
[UsedImplicitly]
internal sealed class WorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.WorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.SelfEmployedDuration)]
[UsedImplicitly]
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
[UsedImplicitly]
internal sealed class YearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.YearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UniversalCredit)]
[UsedImplicitly]
internal sealed class UniversalCreditPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UniversalCredit;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.Benefits)]
[UsedImplicitly]
internal sealed class BenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.Benefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.ChildcareSupport)]
[UsedImplicitly]
internal sealed class ChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.ChildcareVoucherReceipt)]
[UsedImplicitly]
internal sealed class ChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.ChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.UserSettledStatus)]
[UsedImplicitly]
internal sealed class UserSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.UserSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.TypeOfLeave)]
[UsedImplicitly]
internal sealed class TypeOfLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.TypeOfLeave;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
