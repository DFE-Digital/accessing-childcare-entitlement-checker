using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Responses;
using Dfe.Acec.RulesEngine.Evaluators;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.RulesEngine.Schemes;

public class UniversalCreditChildcareEvaluator : ISchemeEvaluator
{
    private const int MinimumEligibleAgeInYears = 0;
    private const int MaximumEligibleAgeInYears = 16;
    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var meetsHouseholdRequirements =
            context.Household is { HasAccessToPublicFunds: true, LivesInGreatBritain: true, ReceivesUniversalCredit: true } &&
            MeetsWorkRequirements(context);

        var eligibleNow =
            meetsHouseholdRequirements &&
            child is { IsBorn: true, AgeInYears: >= MinimumEligibleAgeInYears and <= MaximumEligibleAgeInYears };

        var eligibleInFuture =
            meetsHouseholdRequirements &&
            child is { IsBorn: false, DueDate: not null };

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.UniversalCreditChildcare,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
        };
    }

    private static bool MeetsWorkRequirements(DerivedContext context)
    {
        var userWorking = HasQualifyingPaidWorkStatus(context.User);

        if (!context.Household.HasPartner)
        {
            return userWorking;
        }

        var partnerWorking = context.Partner is not null && HasQualifyingPaidWorkStatus(context.Partner);

        var userExempt =
            HasQualifyingExemptionBenefit(context.User);

        var partnerExempt =
            context.Partner is not null
            && HasQualifyingExemptionBenefit(context.Partner);

        return
            (userWorking && partnerWorking) ||
            (userWorking && partnerExempt) ||
            (partnerWorking && userExempt);
    }

    private static bool HasQualifyingPaidWorkStatus(PersonFacts person)
    {
        return person.PaidWorkStatus is
            PaidWorkStatus.Yes or
            PaidWorkStatus.SickLeave or
            PaidWorkStatus.ParentalLeave;
    }

    private static bool HasQualifyingExemptionBenefit(
        PersonFacts person)
    {
        return person.Benefits.Any(
            _qualifyingExemptionBenefits.Contains);
    }

    private static readonly List<PersonBenefit> _qualifyingExemptionBenefits =
    [
        PersonBenefit.CarersAllowance,
        PersonBenefit.LimitedCapabilityForWork,
        PersonBenefit.LimitedCapabilityForWorkRelatedActivity
    ];
}
