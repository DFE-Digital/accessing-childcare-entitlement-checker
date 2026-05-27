using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class TaxFreeChildcareEvaluator : ISchemeEvaluator
{
    private const int MaximumEligibleAgeInYears = 11;
    private const int MaximumEligibleAgeInYearsWithDisability = 16;
    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {

        var meetsHouseholdRequirements =
            context.Household.HasAccessToPublicFunds &&
            MeetsWorkRequirements(context);

        var eligibleNow =
            meetsHouseholdRequirements &&
            child.IsBorn &&
            ChildIsWithinMaximumEligibleAgeInYears(child);

        var eligibleInFuture =
            meetsHouseholdRequirements &&
            !child.IsBorn;

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.TaxFreeChildcare,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture
        };
    }

    private static bool ChildIsWithinMaximumEligibleAgeInYears(
        ChildFacts child)
    {
        bool childHasEligibleDisability =
            child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.DisabilityLivingAllowance)
            || child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.EducationHealthCarePlan);

        int maximumEligibleAgeInYears = childHasEligibleDisability
            ? MaximumEligibleAgeInYearsWithDisability
            : MaximumEligibleAgeInYears;

        return child.AgeInYears <= maximumEligibleAgeInYears;
    }

    private static bool MeetsWorkRequirements(
        DerivedContext context)
    {
        if (!context.Household.HasPartner)
        {
            return SingleParentMeetsRequirements(
                context.User);
        }

        return CoupleMeetsRequirements(
            context.User,
            context.Partner!);
    }

    private static bool SingleParentMeetsRequirements(
        PersonFacts person)
    {
        return
            person.IsInPaidWork &&
            MeetsMinimumIncomeRequirement(person) &&
            !person.ExceedsAdjustedNetIncomeLimit &&
            !ReceivesDisqualifyingChildcareSupport(person) &&
            !ReceivesDisqualifyingBenefit(person);
    }

    private static bool CoupleMeetsRequirements(
        PersonFacts user,
        PersonFacts partner)
    {
        var bothWorking =
            user.IsInPaidWork
            && partner.IsInPaidWork;

        if (bothWorking)
        {
            return
                MeetsMinimumIncomeRequirement(user)
                && MeetsMinimumIncomeRequirement(partner)
                && !user.ExceedsAdjustedNetIncomeLimit
                && !partner.ExceedsAdjustedNetIncomeLimit
                && !PersonIsDisqualified(user)
                && !PersonIsDisqualified(partner);
        }

        var userWorkingPartnerExempt =
            user.IsInPaidWork
            && MeetsMinimumIncomeRequirement(user)
            && !user.ExceedsAdjustedNetIncomeLimit
            && !PersonIsDisqualified(user)
            && HasQualifyingExemptionBenefit(partner);

        var partnerWorkingUserExempt =
            partner.IsInPaidWork
            && MeetsMinimumIncomeRequirement(partner)
            && !partner.ExceedsAdjustedNetIncomeLimit
            && !PersonIsDisqualified(partner)
            && HasQualifyingExemptionBenefit(user);

        return
            userWorkingPartnerExempt
            || partnerWorkingUserExempt;
    }

    private static bool MeetsMinimumIncomeRequirement(
        PersonFacts person)
    {
        return
            person.EarnsAboveThreshold
            || person.SelfEmployedLessThan12Months;
    }

    private static bool PersonIsDisqualified(
        PersonFacts person)
    {
        return
            ReceivesDisqualifyingBenefit(person)
            || ReceivesDisqualifyingChildcareSupport(person);
    }

    private static bool ReceivesDisqualifyingBenefit(
        PersonFacts person)
    {
        return
            person.Benefits.Contains(
                PersonBenefit.UniversalCredit);
    }

    private static bool ReceivesDisqualifyingChildcareSupport(
        PersonFacts person)
    {
        return
            person.ChildcareSupport.Contains(
                ChildcareSupport.ChildcareVouchers)

            || person.ChildcareSupport.Contains(
                ChildcareSupport.ChildcareBursaryOrGrant);
    }

    private static bool HasQualifyingExemptionBenefit(
        PersonFacts person)
    {
        return
            person.Benefits.Contains(
                PersonBenefit.CarersAllowanceOrCarerSupportPayment)

            || person.Benefits.Contains(
                PersonBenefit.JobseekersAllowance)

            || person.Benefits.Contains(
                PersonBenefit.EmploymentAndSupportAllowance)

            || person.Benefits.Contains(
                PersonBenefit.NationalInsuranceCreditsForIncapacity)

            || person.Benefits.Contains(
                PersonBenefit.IncapacityBenefit)

            || person.Benefits.Contains(
                PersonBenefit.SevereDisablementAllowance);
    }
}