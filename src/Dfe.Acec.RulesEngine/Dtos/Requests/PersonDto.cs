using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Dtos.Requests;

public class PersonDto
{
    public AgeRange? AgeRange { get; init; }
    public PaidWorkStatus? PaidWorkStatus { get; init; }
    public List<WorkStatus> WorkStatuses { get; init; } = [];
    public bool? SelfEmployedLessThan12Months { get; init; }
    public bool? EarnsAboveThreshold { get; init; }
    public bool? ExceedsAdjustedNetIncomeLimit { get; init; }
    public List<PersonBenefit> Benefits { get; init; } = [];

    public List<ChildcareSupport> ChildcareSupport { get; init; } = [];
    public Nationality? Nationality { get; init; }
    public bool? HasSettledOrPreSettledStatus { get; init; }
}