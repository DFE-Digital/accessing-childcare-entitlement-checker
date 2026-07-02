using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Schemes;

public class TaxFreeChildcareEvaluator : ISchemeEvaluator
{
    private const int MaximumEligibleAgeInYears = 11;
    private const int MaximumEligibleAgeInYearsWithDisability = 16;

    public SchemeResultDto? Evaluate(DerivedContext context, ChildFacts child)
    {
        var parentalLeaveAssessment = AssessParentalLeave(context, child);

        var meetsHouseholdRequirements =
            context.Household.HasAccessToPublicFunds &&
            !context.Household.ReceivesUniversalCredit &&
            MeetsWorkRequirements(context, parentalLeaveAssessment);

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
            EligibleInFuture = eligibleInFuture,
            ApplyAndStartAffectedByParentalLeave = parentalLeaveAssessment.ApplyAndStartAffectedByParentalLeave,
            EligibilityEndsWithParentalLeaveFor = GetEligibilityEndParty(context, parentalLeaveAssessment)
        };
    }

    private static ParentalLeaveAssessment AssessParentalLeave(DerivedContext context, ChildFacts child)
    {
        var userIsOnParentalLeave =
            context.User.PaidWorkStatus ==
            PaidWorkStatus.ParentalLeave;

        var partnerIsOnParentalLeave =
            context.Partner?.PaidWorkStatus ==
            PaidWorkStatus.ParentalLeave;

        var userIsOnLeaveForChild =
            userIsOnParentalLeave &&
            child.UserIsOnParentalLeaveForChild;

        var partnerIsOnLeaveForChild =
            partnerIsOnParentalLeave &&
            child.PartnerIsOnParentalLeaveForChild;

        // A person can use the temporary parental-leave income exemption
        // only for children they are not on leave for.
        var userCanUseTemporaryIncomeExemption =
            userIsOnParentalLeave &&
            !userIsOnLeaveForChild;

        var partnerCanUseTemporaryIncomeExemption =
            partnerIsOnParentalLeave &&
            !partnerIsOnLeaveForChild;

        return new ParentalLeaveAssessment(
            UserCanUseTemporaryIncomeExemption:
                userCanUseTemporaryIncomeExemption,

            PartnerCanUseTemporaryIncomeExemption:
                partnerCanUseTemporaryIncomeExemption,

            ApplyAndStartAffectedByParentalLeave:
                GetParentalLeaveParty(
                    userIsOnLeaveForChild,
                    partnerIsOnLeaveForChild));
    }

    private static bool ChildIsWithinMaximumEligibleAgeInYears(ChildFacts child)
    {
        var childHasEligibleDisability =
            child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.DisabilityLivingAllowance)

            || child.ChildRelatedBenefits.Contains(
                ChildRelatedBenefit.EducationHealthAndCarePlan);

        var maximumEligibleAgeInYears =
            childHasEligibleDisability
                ? MaximumEligibleAgeInYearsWithDisability
                : MaximumEligibleAgeInYears;

        return child.AgeInYears <= maximumEligibleAgeInYears;
    }

    private static bool MeetsWorkRequirements(DerivedContext context, ParentalLeaveAssessment parentalLeaveAssessment)
    {
        if (!context.Household.HasPartner)
        {
            return SingleParentMeetsRequirements(
                context.User,
                parentalLeaveAssessment.UserCanUseTemporaryIncomeExemption);
        }

        return CoupleMeetsRequirements(
            context.User,
            context.Partner!,
            parentalLeaveAssessment);
    }

    private static bool SingleParentMeetsRequirements(PersonFacts person, bool canUseTemporaryIncomeExemption)
    {
        var meetsIncomeRequirement =
            MeetsMinimumIncomeRequirement(person) ||
            canUseTemporaryIncomeExemption;

        return
            HasQualifyingPaidWorkStatus(person) &&
            meetsIncomeRequirement &&
            !person.ExceedsAdjustedNetIncomeLimit &&
            !ReceivesDisqualifyingChildcareSupport(person);
    }

    private static bool CoupleMeetsRequirements(PersonFacts user, PersonFacts partner, ParentalLeaveAssessment parentalLeaveAssessment)
    {
        if (user.ExceedsAdjustedNetIncomeLimit || partner.ExceedsAdjustedNetIncomeLimit || PersonIsDisqualified(user) || PersonIsDisqualified(partner))
        {
            return false;
        }

        var userMeetsWorkingRequirements =
            MeetsWorkingRequirements(user, parentalLeaveAssessment.UserCanUseTemporaryIncomeExemption);

        var partnerMeetsWorkingRequirements =
            MeetsWorkingRequirements(partner, parentalLeaveAssessment.PartnerCanUseTemporaryIncomeExemption);

        var bothMeetWorkingRequirements =
            userMeetsWorkingRequirements &&
            partnerMeetsWorkingRequirements;

        var userWorkingPartnerExempt =
            userMeetsWorkingRequirements &&
            HasQualifyingExemptionBenefit(partner);

        var partnerWorkingUserExempt =
            partnerMeetsWorkingRequirements &&
            HasQualifyingExemptionBenefit(user);

        return
            bothMeetWorkingRequirements ||
            userWorkingPartnerExempt ||
            partnerWorkingUserExempt;
    }

    private static bool MeetsWorkingRequirements(PersonFacts person, bool canUseTemporaryIncomeExemption)
    {
        var meetsIncomeRequirement =
            MeetsMinimumIncomeRequirement(person) ||
            canUseTemporaryIncomeExemption;

        return
            HasQualifyingPaidWorkStatus(person) &&
            meetsIncomeRequirement;
    }

    private static bool HasQualifyingPaidWorkStatus(PersonFacts person)
    {
        return person.PaidWorkStatus is
            PaidWorkStatus.Yes
            or PaidWorkStatus.ParentalLeave
            or PaidWorkStatus.SickLeave;
    }

    private static bool MeetsMinimumIncomeRequirement(PersonFacts person)
    {
        return
            person.EarnsAboveThreshold ||
            person.SelfEmployedLessThan12Months;
    }

    private static ParentalLeaveParty? GetEligibilityEndParty(DerivedContext context, ParentalLeaveAssessment parentalLeaveAssessment)
    {
        var eligibilityDependsOnUserLeave =
            parentalLeaveAssessment
                .UserCanUseTemporaryIncomeExemption
            &&
            !MeetsWorkRequirements(
                context,
                parentalLeaveAssessment with
                {
                    UserCanUseTemporaryIncomeExemption = false
                });

        var eligibilityDependsOnPartnerLeave =
            parentalLeaveAssessment
                .PartnerCanUseTemporaryIncomeExemption
            &&
            !MeetsWorkRequirements(
                context,
                parentalLeaveAssessment with
                {
                    PartnerCanUseTemporaryIncomeExemption = false
                });

        return GetParentalLeaveParty(
            eligibilityDependsOnUserLeave,
            eligibilityDependsOnPartnerLeave);
    }

    private static ParentalLeaveParty? GetParentalLeaveParty(bool appliesToUser, bool appliesToPartner)
    {
        return (appliesToUser, appliesToPartner) switch
        {
            (true, true) => ParentalLeaveParty.UserAndPartner,

            (true, false) => ParentalLeaveParty.User,

            (false, true) => ParentalLeaveParty.Partner,

            _ => null
        };
    }

    private static bool PersonIsDisqualified(PersonFacts person)
    {
        return ReceivesDisqualifyingChildcareSupport(person);
    }

    private static bool ReceivesDisqualifyingChildcareSupport(PersonFacts person)
    {
        return
            person.ChildcareSupport.Contains(
                ChildcareSupport.ChildcareVouchers)

            || person.ChildcareSupport.Contains(
                ChildcareSupport.ChildcareBursaryOrGrant);
    }

    private static bool HasQualifyingExemptionBenefit(PersonFacts person)
    {
        return person.Benefits.Any(
            QualifyingExemptionBenefits.Contains);
    }

    private static readonly List<PersonBenefit>
        QualifyingExemptionBenefits =
    [
        PersonBenefit.CarersAllowance,
        PersonBenefit.ContributionBasedEmploymentAndSupportAllowance,
        PersonBenefit.IncapacityBenefit,
        PersonBenefit.SevereDisablementAllowance
    ];

    private sealed record ParentalLeaveAssessment(
        bool UserCanUseTemporaryIncomeExemption,
        bool PartnerCanUseTemporaryIncomeExemption,
        ParentalLeaveParty? ApplyAndStartAffectedByParentalLeave);
}