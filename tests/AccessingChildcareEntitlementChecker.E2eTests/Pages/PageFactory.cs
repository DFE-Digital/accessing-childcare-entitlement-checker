using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal class PageFactory(IPage page)
{
    public IPageObject GetPage(string pageNameOrHeading)
    {
        var cleaned = pageNameOrHeading.Trim();

        // Handle dynamic names first
        if (cleaned.Contains(PageNames.ChildBirthDate, StringComparison.OrdinalIgnoreCase))
        {
            return new ChildBirthDatePage(page);
        }
        if (cleaned.Contains(PageNames.ChildRelationship, StringComparison.OrdinalIgnoreCase))
        {
            return new ChildRelationshipPage(page);
        }
        if (cleaned.Contains(PageNames.ChildSupport, StringComparison.OrdinalIgnoreCase))
        {
            return new ChildSupportPage(page);
        }
        if (cleaned.Contains(PageNames.ChildDueDate, StringComparison.OrdinalIgnoreCase))
        {
            return new ChildDueDatePage(page);
        }

        return cleaned switch
        {
            PageNames.StartPage => new StartPage(page),
            PageNames.Location => new LocationPage(page),
            PageNames.UserAge => new UserAgePage(page),
            PageNames.Nationality => new NationalityPage(page),
            PageNames.PaidWork => new PaidWorkPage(page),
            PageNames.WorkStatus => new WorkStatusPage(page),
            PageNames.SelfEmployedDuration => new SelfEmployedDurationPage(page),
            PageNames.WeeklyEarnings => new WeeklyEarningsPage(page),
            PageNames.YearlyEarnings => new YearlyEarningsPage(page),
            PageNames.UniversalCredit => new UniversalCreditPage(page),
            PageNames.Benefits => new BenefitsPage(page),
            PageNames.ChildcareSupport => new ChildcareSupportPage(page),
            PageNames.ChildcareVoucherReceipt => new ChildcareVoucherReceiptPage(page),
            PageNames.UserSettledStatus => new UserSettledStatusPage(page),
            PageNames.TypeOfLeave => new TypeOfLeavePage(page),

            PageNames.HasPartner => new HasPartnerPage(page),
            PageNames.PartnerAge => new PartnerAgePage(page),
            PageNames.PartnerPaidWork => new PartnerPaidWorkPage(page),
            PageNames.PartnerWorkStatus => new PartnerWorkStatusPage(page),
            PageNames.PartnerSelfEmployedDuration => new PartnerSelfEmployedDurationPage(page),
            PageNames.PartnerWeeklyEarnings => new PartnerWeeklyEarningsPage(page),
            PageNames.PartnerYearlyEarnings => new PartnerYearlyEarningsPage(page),
            PageNames.PartnerNationality => new PartnerNationalityPage(page),
            PageNames.PartnerSettledStatus => new PartnerSettledStatusPage(page),
            PageNames.PartnerBenefits => new PartnerBenefitsPage(page),
            PageNames.PartnerChildcareSupport => new PartnerChildcareSupportPage(page),
            PageNames.PartnerChildcareVoucherReceipt => new PartnerChildcareVoucherReceiptPage(page),
            PageNames.PartnerLeaveType => new PartnerLeaveTypePage(page),

            PageNames.ChildName => new ChildNamePage(page),
            PageNames.ChildIsBorn => new ChildIsBornPage(page),

            _ => throw new KeyNotFoundException($"No Page Object mapped for page: '{pageNameOrHeading}'")
        };
    }
}
