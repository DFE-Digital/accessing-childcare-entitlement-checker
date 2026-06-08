using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class FifteenHoursForDisadvantagedChildrenEvaluator : ISchemeEvaluator
{
    private const int MaximumEligibleAgeInYears = 2;
    private const int WeeksBeforeTermStartParentsCanApply = 16;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var eligibleNow =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            child.IsBorn &&
            child.AgeInYears == MaximumEligibleAgeInYears &&
            (
                ChildMeetsAutomaticEligibilityCriteria(child)
                || HouseholdMeetsBenefitEligibility(context)
            );


        var eligibleInFuture =
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            (
                !child.IsBorn ||
                child.AgeInYears is < MaximumEligibleAgeInYears
            )
            &&
                HouseholdMeetsBenefitEligibility(context)
            ;

        if (!eligibleNow && !eligibleInFuture)
        {
            return null;
        }

        var secondBirthdayDate =
            child.IsBorn
                ? child.DateOfBirth?.AddYears(2)
                : child.DueDate?.AddYears(2);

        DateOnly? useFromDate =
            secondBirthdayDate is not null
                ? TermDateCalculator.GetNextTermStartDate(
                    secondBirthdayDate.Value)
                : null;

        DateOnly? applyFromDate =
            useFromDate?.AddDays(-(WeeksBeforeTermStartParentsCanApply * 7));

        return new SchemeResultDto
        {
            SchemeCode = SchemeCode.FifteenHoursForDisadvantagedChildren,
            EligibleNow = eligibleNow,
            EligibleInFuture = eligibleInFuture,
            ApplyFromDate = applyFromDate,
            UseFromDate = useFromDate
        };
    }

    private static bool ChildMeetsAutomaticEligibilityCriteria(
        ChildFacts child)
    {
        return
            child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.DisabilityLivingAllowance)

            || child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.EducationHealthAndCarePlan)

            || child.RelationshipToChild == RelationshipToChild.FosterParent;
    }

    private static bool HouseholdMeetsBenefitEligibility(
        DerivedContext context)
    {
        return
            context.Household.CountryOfResidence == CountryOfResidence.England &&
            context.Household.HasAccessToPublicFunds &&
            HouseholdReceivesQualifyingBenefit(context);
    }

    private static bool HouseholdReceivesQualifyingBenefit(
        DerivedContext context)
    {
        return
            context.Household.ReceivesUniversalCredit || 
            PersonReceivesQualifyingBenefit(context.User) || 
            context.Partner is not null && PersonReceivesQualifyingBenefit(context.Partner);
    }

    private static bool PersonReceivesQualifyingBenefit(
        PersonFacts person)
    {
        return person.Benefits.Any(
            QualifyingBenefits.Contains);
    }

    private static readonly List<PersonBenefit> QualifyingBenefits =
    [
        PersonBenefit.ContributionBasedEmploymentAndSupportAllowance,
        PersonBenefit.GuaranteedElementOfPensionCredit
    ];
}