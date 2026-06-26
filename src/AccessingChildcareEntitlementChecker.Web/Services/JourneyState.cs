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

    public List<string> ParentalLeaveChildrenIds { get; set; } = [];

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

    public List<string> PartnerParentalLeaveChildrenIds { get; set; } = [];

    public SelfEmployedDurationOption? PartnerSelfEmployedDuration { get; set; }

    public WeeklyEarningsOption? PartnerWeeklyEarnings { get; set; }

    public YearlyEarningsOption? PartnerYearlyEarnings { get; set; }

    public List<PartnerBenefitsOption> PartnerBenefits { get; set; } = [];

    public List<PartnerChildcareSupportOption> PartnerChildcareSupport { get; set; } = [];

    public ChildcareVoucherReceiptOption? PartnerChildcareVoucherReceipt { get; set; }

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

        if (!Children.TryGetValue(model.ChildId, out var child))
        {
            child = new Child(model.ChildId, model.ChildName);
            Children.Add(model.ChildId, child);
            return;
        }

        child.Name = model.ChildName;
    }

    public void Apply(ChildIsBornViewModel model)
    {
        if (!Children.TryGetValue(model.ChildId, out var child))
        {
            throw new InvalidOperationException("Child not found");
        }

        child.BirthStatus = model.ChildIsBorn;

        if (child.BirthStatus == BirthStatus.Born)
        {
            child.DueDate = null;
            child.ExpectedRelationship = null;
        }
        else
        {
            child.BirthDate = null;
            child.BornRelationship = null;
            child.ChildSupportOptions = [];
        }
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
        if (model.UserAge != UserAge)
        {
            WeeklyEarnings = null;
            YearlyEarnings = null;
        }

        UserAge = model.UserAge;
    }

    public void Apply(NationalityViewModel model)
    {
        if (model.Nationality != Nationality)
        {
            SettledStatus = null;
            PartnerNationality = null;
            PartnerSettledStatus = null;
        }

        Nationality = model.Nationality;
    }

    public void Apply(SettledStatusViewModel model)
    {
        SettledStatus = model.SettledStatus;
    }

    public void Apply(PaidWorkViewModel model)
    {
        if (model.PaidWork == PaidWorkOption.Yes)
        {
            ParentalLeaveChildrenIds = [];
        }

        if (model.PaidWork == PaidWorkOption.ParentalLeave)
        {
            WeeklyEarnings = null;
        }

        if (model.PaidWork == PaidWorkOption.SickLeave)
        {
            ParentalLeaveChildrenIds = [];
            WeeklyEarnings = null;
        }

        if (model.PaidWork == PaidWorkOption.No)
        {
            WorkStatus = [];
            ParentalLeaveChildrenIds = [];
            SelfEmployedDuration = null;
            WeeklyEarnings = null;
            YearlyEarnings = null;
        }

        PaidWork = model.PaidWork;
    }

    public void Apply(ParentalLeaveViewModel model)
    {
        ParentalLeaveChildrenIds = model.ParentalLeaveChildrenIds;
    }

    public void Apply(WorkStatusViewModel model)
    {
        if (!model.WorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            SelfEmployedDuration = null;
        }

        WorkStatus = model.WorkStatus;
    }

    public void Apply(SelfEmployedDurationViewModel model)
    {
        if (model.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            WeeklyEarnings = null;
            YearlyEarnings = null;
        }

        SelfEmployedDuration = model.SelfEmployedDuration;
    }

    public void Apply(WeeklyEarningsViewModel model)
    {
        if (model.WeeklyEarnings == WeeklyEarningsOption.BelowThreshold)
        {
            YearlyEarnings = null;
        }

        WeeklyEarnings = model.WeeklyEarnings;
    }

    public void Apply(YearlyEarningsViewModel model)
    {
        if (model.YearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            UniversalCredit = null;
        }

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
        if (!model.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers))
        {
            ChildcareVoucherReceipt = null;
        }

        ChildcareSupport = model.ChildcareSupport;
    }

    public void Apply(ChildcareVoucherReceiptViewModel model)
    {
        ChildcareVoucherReceipt = model.ChildcareVoucherReceipt;
    }

    public void Apply(HasPartnerViewModel model)
    {
        if (model.HasPartner == false)
        {
            PartnerAge = null;
            PartnerNationality = null;
            PartnerSettledStatus = null;
            PartnerPaidWork = null;
            PartnerWorkStatus = [];
            PartnerSelfEmployedDuration = null;
            PartnerWeeklyEarnings = null;
            PartnerYearlyEarnings = null;
            PartnerBenefits = [];
            PartnerChildcareSupport = [];
            PartnerChildcareVoucherReceipt = null;
        }

        HasPartner = model.HasPartner;
    }

    public void Apply(PartnerAgeViewModel model)
    {
        if (model.PartnerAge != PartnerAge)
        {
            PartnerWeeklyEarnings = null;
            PartnerYearlyEarnings = null;
        }

        PartnerAge = model.PartnerAge;
    }

    public void Apply(PartnerNationalityViewModel model)
    {
        if (model.PartnerNationality != PartnerNationality)
        {
            PartnerSettledStatus = null;
        }

        PartnerNationality = model.PartnerNationality;
    }

    public void Apply(PartnerSettledStatusViewModel model)
    {
        PartnerSettledStatus = model.PartnerSettledStatus;
    }

    public void Apply(PartnerPaidWorkViewModel model)
    {
        if (model.PartnerPaidWork == PartnerPaidWorkOption.Yes)
        {
            PartnerParentalLeaveChildrenIds = [];
        }

        if (model.PartnerPaidWork == PartnerPaidWorkOption.ParentalLeave)
        {
            PartnerWeeklyEarnings = null;
        }

        if (model.PartnerPaidWork == PartnerPaidWorkOption.SickLeave)
        {
            PartnerParentalLeaveChildrenIds = [];
            PartnerWeeklyEarnings = null;
        }

        if (model.PartnerPaidWork == PartnerPaidWorkOption.No)
        {
            PartnerWorkStatus = [];
            PartnerParentalLeaveChildrenIds = [];
            PartnerSelfEmployedDuration = null;
            PartnerWeeklyEarnings = null;
            PartnerYearlyEarnings = null;
        }

        PartnerPaidWork = model.PartnerPaidWork;
    }

    public void Apply(PartnerParentalLeaveViewModel model)
    {
        PartnerParentalLeaveChildrenIds = model.PartnerParentalLeaveChildrenIds;
    }

    public void Apply(PartnerWorkStatusViewModel model)
    {
        if (!model.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed))
        {
            PartnerSelfEmployedDuration = null;
        }

        PartnerWorkStatus = model.PartnerWorkStatus;
    }

    public void Apply(PartnerSelfEmployedDurationViewModel model)
    {
        if (model.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            PartnerWeeklyEarnings = null;
            PartnerYearlyEarnings = null;
        }

        PartnerSelfEmployedDuration = model.PartnerSelfEmployedDuration;
    }

    public void Apply(PartnerWeeklyEarningsViewModel model)
    {
        if (model.PartnerWeeklyEarnings == WeeklyEarningsOption.BelowThreshold)
        {
            PartnerYearlyEarnings = null;
        }

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
        if (!model.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
        {
            PartnerChildcareVoucherReceipt = null;
        }

        PartnerChildcareSupport = model.PartnerChildcareSupport;
    }

    public void Apply(PartnerChildcareVoucherReceiptViewModel model)
    {
        PartnerChildcareVoucherReceipt = model.PartnerChildcareVoucherReceipt;
    }
}
