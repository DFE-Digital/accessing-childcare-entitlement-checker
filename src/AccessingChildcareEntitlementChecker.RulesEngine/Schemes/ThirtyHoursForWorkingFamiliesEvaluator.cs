using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;


namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class ThirtyHoursForWorkingFamiliesEvaluator : ISchemeEvaluator
{
    private const int ApplyAgeInWeeks = 23;
    private const int MinimumEligibleAgeInMonths = 9;
    private const int MaximumEligibleAgeInYears = 4;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var meetsHouseholdRequirements =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            context.Household.HasAccessToPublicFunds &&
            MeetsWorkRequirements(context);

        var eligibleNow =
            meetsHouseholdRequirements &&
            child.IsBorn &&
            child.AgeInMonths is >= MinimumEligibleAgeInMonths &&
            child.AgeInYears is <= MaximumEligibleAgeInYears;

        var eligibleInFuture =
            meetsHouseholdRequirements &&
            ((child.IsBorn && child.AgeInMonths is < MinimumEligibleAgeInMonths) || !child.IsBorn);

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        DateOnly? applyFromDate =
            child.IsBorn
                ? child.DateOfBirth?.AddDays(ApplyAgeInWeeks * 7)
                : child.DueDate?.AddDays(ApplyAgeInWeeks * 7);

        DateOnly? nineMonthsOldDate =
            child.IsBorn
                ? child.DateOfBirth?.AddMonths(MinimumEligibleAgeInMonths)
                : child.DueDate?.AddMonths(MinimumEligibleAgeInMonths);

        DateOnly? useFromDate =
            nineMonthsOldDate is not null
                ? TermDateCalculator.GetNextTermStartDate(
                    nineMonthsOldDate.Value)
                : null;

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
            ApplyFromDate = applyFromDate,
            UseFromDate = useFromDate
        };
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
            person.IsInPaidWork
            && MeetsMinimumIncomeRequirement(person)
            && !person.ExceedsAdjustedNetIncomeLimit;
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
                && !partner.ExceedsAdjustedNetIncomeLimit;
        }

        var userWorkingPartnerExempt =
            user.IsInPaidWork
            && MeetsMinimumIncomeRequirement(user)
            && !user.ExceedsAdjustedNetIncomeLimit
            && HasQualifyingExemptionBenefit(partner);

        var partnerWorkingUserExempt =
            partner.IsInPaidWork
            && MeetsMinimumIncomeRequirement(partner)
            && !partner.ExceedsAdjustedNetIncomeLimit
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

    private static bool HasQualifyingExemptionBenefit(
        PersonFacts person)
    {
        return person.Benefits.Any(
            QualifyingExemptionBenefits.Contains);
    }

    private static readonly List<PersonBenefit> QualifyingExemptionBenefits =
    [
        PersonBenefit.CarersAllowanceOrCarerSupportPayment,
        PersonBenefit.JobseekersAllowance,
        PersonBenefit.EmploymentAndSupportAllowance,
        PersonBenefit.NationalInsuranceCreditsForIncapacity,
        PersonBenefit.IncapacityBenefit,
        PersonBenefit.SevereDisablementAllowance
    ];

}