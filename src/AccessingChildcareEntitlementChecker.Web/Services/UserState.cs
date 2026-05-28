using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;

public class UserState
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public AgeRange? UserAge { get; set; }

    public NationalityOption? Nationality { get; set; }

    public PaidWorkOption? PaidWork { get; set; }

    public SettledStatusOption? SettledStatus { get; set; }

    public List<WorkStatusOption> WorkStatus { get; set; } = [];

    public SelfEmployedDurationOption? SelfEmployedDuration { get; set; }

    public WeeklyEarningsOption? WeeklyEarnings { get; set; }

    public YearlyEarningsOption? YearlyEarnings { get; set; }

    public UniversalCreditOption? UniversalCredit { get; set; }

    public List<BenefitsOption> Benefits { get; set; } = [];

    public List<ChildcareSupportOption> ChildcareSupport { get; set; } = [];

    public ChildcareVoucherReceiptOption? ChildcareVoucherReceipt { get; set; }

    public HasPartnerOption? HasPartner { get; set; }

    public void Apply(LocationViewModel model)
    {
        CountryOfResidence = model.Country;
    }

    public void Apply(UserAgeViewModel model)
    {
        UserAge = model.UserAge;
    }

    public void Apply(NationalityViewModel model)
    {
        Nationality = model.Nationality;
    }

    public void Apply(SettledStatusViewModel model)
    {
        SettledStatus = model.SettledStatus;
    }

    public void Apply(PaidWorkViewModel model)
    {
        PaidWork = model.PaidWork;
    }

    public void Apply(WorkStatusViewModel model)
    {
        WorkStatus = model.WorkStatus;
    }

    public void Apply(SelfEmployedDurationViewModel model)
    {
        SelfEmployedDuration = model.SelfEmployedDuration;
    }

    public void Apply(WeeklyEarningsViewModel model)
    {
        WeeklyEarnings = model.WeeklyEarnings;
    }

    public void Apply(YearlyEarningsViewModel model)
    {
        YearlyEarnings = model.YearlyEarnings;
    }

    public void Apply(UniversalCreditViewModel model)
    {
        UniversalCredit = model.UniversalCredit;
    }

    public void Apply(BenefitsViewModel model)
    {
        Benefits = model.Benefits;
    }

    public void Apply(ChildcareSupportViewModel model)
    {
        ChildcareSupport = model.ChildcareSupport;
    }

    public void Apply(ChildcareVoucherReceiptViewModel model)
    {
        ChildcareVoucherReceipt = model.ChildcareVoucherReceipt;
    }

    public void Apply(HasPartnerViewModel model)
    {
        HasPartner = model.HasPartner;
    }
}
