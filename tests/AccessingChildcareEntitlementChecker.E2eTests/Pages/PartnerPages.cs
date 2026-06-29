using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal class HasPartnerPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.HasPartner;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerPaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerPaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerBenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerBenefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class PartnerChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class PartnerChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerWorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

internal class PartnerSelfEmployedDurationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSelfEmployedDuration;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerWeeklyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerYearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerYearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerNationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerNationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

internal class PartnerLeaveTypePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerLeaveType;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
