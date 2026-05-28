using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;

namespace AccessingChildcareEntitlementChecker.Web.Services;
public class PartnerState
{
    public AgeRange? PartnerAge { get; set; }

    public NationalityOption? PartnerNationality { get; set; }

    public SettledStatusOption? PartnerSettledStatus { get; set; }

    public PartnerPaidWorkOption? PartnerPaidWork { get; set; }

    public List<WorkStatusOption> PartnerWorkStatus { get; set; } = [];

    public SelfEmployedDurationOption? PartnerSelfEmployedDuration { get; set; }

    public WeeklyEarningsOption? PartnerWeeklyEarnings { get; set; }

    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }

    public List<PartnerBenefitsOption> PartnerBenefits { get; set; } = [];

    public List<PartnerChildcareSupportOption> PartnerChildcareSupport { get; set; } = [];

    public ChildcareVoucherReceiptOption? PartnerChildcareVoucherReceipt { get; set; }

    public void Apply(PartnerAgeViewModel model)
    {
        PartnerAge = model.PartnerAge;
    }

    public void Apply(PartnerNationalityViewModel model)
    {
        PartnerNationality = model.PartnerNationality;
    }

    public void Apply(PartnerSettledStatusViewModel model)
    {
        PartnerSettledStatus = model.PartnerSettledStatus;
    }

    public void Apply(PartnerPaidWorkViewModel model)
    {
        PartnerPaidWork = model.PartnerPaidWork;
    }

    public void Apply(PartnerWorkStatusViewModel model)
    {
        PartnerWorkStatus = model.PartnerWorkStatus;
    }

    public void Apply(PartnerSelfEmployedDurationViewModel model)
    {
        PartnerSelfEmployedDuration = model.PartnerSelfEmployedDuration;
    }

    public void Apply(PartnerWeeklyEarningsViewModel model)
    {
        PartnerWeeklyEarnings = model.PartnerWeeklyEarnings;
    }

    public void Apply(PartnerYearlyEarningsViewModel model)
    {
        PartnerYearlyEarnings = model.PartnerYearlyEarnings;
    }

    public void Apply(PartnerBenefitsViewModel model)
    {
        PartnerBenefits = model.PartnerBenefits;
    }

    public void Apply(PartnerChildcareSupportViewModel model)
    {
        PartnerChildcareSupport = model.PartnerChildcareSupport;
    }

    public void Apply(PartnerChildcareVoucherReceiptViewModel model)
    {
        PartnerChildcareVoucherReceipt = model.PartnerChildcareVoucherReceipt;
    }
}