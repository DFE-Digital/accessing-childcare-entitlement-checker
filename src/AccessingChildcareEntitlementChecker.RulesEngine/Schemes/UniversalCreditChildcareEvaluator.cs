using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class UniversalCreditChildcareEvaluator : ISchemeEvaluator
{
    private const int MinimumEligibleAgeInYears = 0;
    private const int MaximumEligibleAgeInYears = 16;
    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var meetsHouseholdRequirements =
            context.Household.HasAccessToPublicFunds &&
            context.Household.LivesInGreatBritain &&
            context.Household.ReceivesUniversalCredit &&
            MeetsWorkRequirements(context);

        var eligibleNow =
            meetsHouseholdRequirements &&
            child.IsBorn &&
            child.AgeInYears is >= MinimumEligibleAgeInYears and <= MaximumEligibleAgeInYears;

        var eligibleInFuture =
            meetsHouseholdRequirements &&
            !child.IsBorn &&
            child.DueDate is not null;

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
            QualifyingExemptionBenefits.Contains);
    }

    private static readonly List<PersonBenefit> QualifyingExemptionBenefits =
    [
        PersonBenefit.CarersAllowance,
        PersonBenefit.LimitedCapabilityForWork,
        PersonBenefit.LimitedCapabilityForWorkRelatedActivity
    ];
}