using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Responses;
using Dfe.Acec.RulesEngine.Evaluators;
using Dfe.Acec.RulesEngine.Helpers;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Schemes;

public class FifteenHoursUniversalEvaluator : ISchemeEvaluator
{
    private const int _minimumEligibleAgeInYears = 3;
    private const int _maximumEligibleAgeInYears = 4;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var eligibleNow =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            child.IsBorn &&
            child.AgeInYears is >= _minimumEligibleAgeInYears and <= _maximumEligibleAgeInYears;

        var eligibleInFuture =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            ((child.IsBorn && child.AgeInYears is < _minimumEligibleAgeInYears) || !child.IsBorn);

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        var thirdBirthdayDate =
            child.IsBorn
                ? child.DateOfBirth?.AddYears(_minimumEligibleAgeInYears)
                : child.DueDate?.AddYears(_minimumEligibleAgeInYears);


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
