using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class FifteenHoursUniversalEvaluator : ISchemeEvaluator
{
    private const int MinimumEligibleAgeInYears = 3;
    private const int MaximumEligibleAgeInYears = 4;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var eligibleNow =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            child.IsBorn &&
            child.AgeInYears is >= MinimumEligibleAgeInYears and <= MaximumEligibleAgeInYears;

        var eligibleInFuture =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            ((child.IsBorn && child.AgeInYears is < MinimumEligibleAgeInYears) || !child.IsBorn);

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        var thirdBirthdayDate =
            child.IsBorn
                ? child.DateOfBirth?.AddYears(MinimumEligibleAgeInYears)
                : child.DueDate?.AddYears(MinimumEligibleAgeInYears);


        DateOnly? useFromDate =
            thirdBirthdayDate is not null
                ? TermDateCalculator.GetNextTermStartDate(
                    thirdBirthdayDate.Value)
                : null;

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.FifteenHoursUniversal,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
            UseFromDate = useFromDate
        };
    }
}