using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

[PagePattern(PageNames.HasPartner)]
internal class HasPartnerPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.HasPartner;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerAge)]
internal class PartnerAgePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerAge;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerPaidWork)]
internal class PartnerPaidWorkPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerPaidWork;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerParentalLeave)]
internal class PartnerParentalLeavePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerParentalLeave;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerBenefits)]
internal class PartnerBenefitsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerBenefits;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerChildcareSupport)]
internal class PartnerChildcareSupportPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareSupport;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerChildcareVoucherReceipt)]
internal class PartnerChildcareVoucherReceiptPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerChildcareVoucherReceipt;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerWorkStatus)]
internal class PartnerWorkStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWorkStatus;
    public override async Task AnswerAsync(string answer) => await CheckCheckboxesAsync(answer);
}

[PagePattern(PageNames.PartnerSelfEmployedDuration)]
internal class PartnerSelfEmployedDurationPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSelfEmployedDuration;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerWeeklyEarnings)]
internal class PartnerWeeklyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerWeeklyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerYearlyEarnings)]
internal class PartnerYearlyEarningsPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerYearlyEarnings;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerNationality)]
internal class PartnerNationalityPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerNationality;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerSettledStatus)]
internal class PartnerSettledStatusPage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerSettledStatus;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}

[PagePattern(PageNames.PartnerLeaveType)]
internal class PartnerLeaveTypePage(IPage page) : BasePage(page)
{
    public override string PageTitle => PageNames.PartnerLeaveType;
    public override async Task AnswerAsync(string answer) => await SelectRadioAsync(answer);
}
