using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class FifteenHoursUniversalEvaluator : ISchemeEvaluator
{
    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var eligibleNow =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            child.IsBorn &&
            child.AgeInYears is >= 3 and <= 4;

        var eligibleInFuture =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            ((child.IsBorn && child.AgeInYears is < 3) || !child.IsBorn);

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        var thirdBirthday =
            child.IsBorn
                ? child.DateOfBirth?.AddYears(3)
                : child.DueDate?.AddYears(3);

        DateOnly? applyFromDate = thirdBirthday;

        DateOnly? useFromDate =
            thirdBirthday is not null
                ? TermDateCalculator.GetNextTermStartDate(
                    thirdBirthday.Value)
                : null;

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.FifteenHoursForWorkingFamilies,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
            EligibleWhenChildTurns = 3,
            ApplyFromDate = applyFromDate,
            UseFromDate = useFromDate
        };
    }
}