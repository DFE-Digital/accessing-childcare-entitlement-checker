using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class UniversalCreditChildcareEvaluator : ISchemeEvaluator
{
    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var eligibleNow =
            context.Household.HasAccessToPublicFunds &&
            context.Household.LivesInGreatBritain &&
            context.Household.ReceivesUniversalCredit &&
            MeetsWorkRequirements(context) &&
            child.IsBorn &&
            child.AgeInYears <= 16;

        var eligibleInFuture =
            context.Household.HasAccessToPublicFunds &&
            context.Household.LivesInGreatBritain &&
            context.Household.ReceivesUniversalCredit &&
            MeetsWorkRequirements(context) &&
            !child.IsBorn &&
            child.DueDate is not null;

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        var eligibleFrom =
            eligibleInFuture
                ? child.DueDate
                : null;

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.UniversalCreditChildcare,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
            EligibleFrom = eligibleFrom
        };
    }

    private bool MeetsWorkRequirements(DerivedContext context)
    {
        if (!context.Household.HasPartner)
        {
            return context.User.IsInPaidWork;
        }

        var userWorking =
            context.User.IsInPaidWork;

        var partnerWorking =
            context.Partner?.IsInPaidWork == true;

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

    private static bool HasQualifyingExemptionBenefit(
        PersonFacts person)
    {
        return person.Benefits.Contains(
                   PersonBenefit.CarersAllowanceOrCarerSupportPayment)

               || person.Benefits.Contains(
                   PersonBenefit.IncapacityBenefit)

               || person.Benefits.Contains(
                   PersonBenefit.NationalInsuranceCreditsForIncapacity)

               || person.Benefits.Contains(
                   PersonBenefit.SevereDisablementAllowance);
    }
}