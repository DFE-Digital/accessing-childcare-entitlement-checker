using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{

    public PaidWorkOption? PaidWork { get; set; }

    public SettledStatusOption? SettledStatus { get; set; }

    public CountryOfResidence? CountryOfResidence { get; set; }

    public Dictionary<string, Child> Children { get; set; } = [];

    public AgeRange? UserAge { get; set; }

    public NationalityOption? Nationality { get; set; }

    public List<WorkStatusOption> WorkStatus { get; set; } = [];

    public SelfEmployedDurationOption? SelfEmployedDuration { get; set; }

    public WeeklyEarningsOption? WeeklyEarnings { get; set; }

    public YearlyEarningsOption? YearlyEarnings { get; set; }

    public UniversalCreditOption? UniversalCredit { get; set; }

    public List<BenefitsOption> Benefits { get; set; } = [];

    public List<ChildcareSupportOption> ChildcareSupport { get; set; } = [];

    public ChildcareVoucherReceiptOption? ChildcareVoucherReceipt { get; set; }

    public bool? HasPartner { get; set; }

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

    public Child? GetChild(string childId)
    {
        return Children.TryGetValue(childId, out var child) ? child : null;
    }

    public void Apply(LocationViewModel model)
    {
        CountryOfResidence = model.Country;
    }

    public void Apply(ChildNameViewModel model)
    {
        if (model.ChildName == null)
        {
            throw new InvalidOperationException("Child name cannot be null");
        }

        if (model.ChildId == null)
        {
            model.ChildId = Guid.NewGuid().ToString();
        }

        var child = GetChild(model.ChildId);
        if (child == null)
        {
            child = new Child(model.ChildId, model.ChildName);
            Children.Add(model.ChildId, child);
            return;
        }

        child.Name = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        var child = Children[model.ChildId];
        child.BirthStatus = model.ChildIsBorn;
    }

    public void Apply(ChildBirthDateViewModel model)
    {
        var child = Children[model.ChildId];
        child.BirthDate = model.ChildBirthDate;
    }

    public void Apply(ChildDueDateViewModel model)
    {
        var child = Children[model.ChildId];
        child.DueDate = model.ChildDueDate;
    }

    public void Apply(ChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId];
        child.BornRelationship = model.Relationship;
    }

    public void Apply(ChildSupportViewModel model)
    {
        var child = Children[model.ChildId];
        child.ChildSupportOptions = model.ChildSupportOptions;
    }

    public void Apply(ExpectedChildRelationshipViewModel model)
    {
        var child = Children[model.ChildId];
        child.ExpectedRelationship = model.ExpectedChildRelationship;
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
