using Microsoft.Playwright;

namespace Dfe.Acec.Web.Tests.E2e.Pages;

[PagePattern(PageNames.HasPartner)]
internal sealed class HasPartnerPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.HasPartner;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerAge)]
internal sealed class PartnerAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerPaidWork)]
internal sealed class PartnerPaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerPaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerParentalLeave)]
internal sealed class PartnerParentalLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerParentalLeave;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerBenefits)]
internal sealed class PartnerBenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerBenefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerChildcareSupport)]
internal sealed class PartnerChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerChildcareVoucherReceipt)]
internal sealed class PartnerChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerWorkStatus)]
internal sealed class PartnerWorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerSelfEmployedDuration)]
internal sealed class PartnerSelfEmployedDurationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSelfEmployedDuration;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerWeeklyEarnings)]
internal sealed class PartnerWeeklyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerYearlyEarnings)]
internal sealed class PartnerYearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerYearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerNationality)]
internal sealed class PartnerNationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerNationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerSettledStatus)]
internal sealed class PartnerSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerLeaveType)]
internal sealed class PartnerLeaveTypePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerLeaveType;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerLeaveWeeklyEarnings)]
internal sealed class PartnerLeaveWeeklyEarnings(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerLeaveWeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
