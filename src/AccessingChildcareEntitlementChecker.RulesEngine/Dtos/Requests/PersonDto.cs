using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;

public class PersonDto
{
    public AgeRange? AgeRange { get; set; }

    public bool? IsInPaidWork { get; set; }

    public WorkStatus? WorkStatus { get; set; }

    public bool? SelfEmployedLessThan12Months { get; set; }

    public bool? EarnsAboveThreshold { get; set; }

    public bool? AdjustedNetIncomeOver100K { get; set; }

    public List<PersonBenefit> Benefits { get; set; } = [];

    public List<ChildcareSupport> ChildcareSupport { get; set; } = [];

    public Nationality? Nationality { get; set; }

    public bool? HasSettledOrPreSettledStatus { get; set; }

    public bool? HasAccessToPublicFunds { get; set; }
}