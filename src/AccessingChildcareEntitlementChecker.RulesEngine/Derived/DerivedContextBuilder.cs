using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Derived;

public class DerivedContextBuilder
{

    public DerivedContext Build(EntitlementRequest request, DateOnly today)
    {
        var user = BuildPersonFacts(request.User);
        var partner = request.Partner is null ? null : BuildPersonFacts(request.Partner);
        var children = request.Children
            .Select(child => BuildChildFacts(child, today))
            .ToList();
        var household = BuildHouseholdFacts(request, user, partner);

        return new DerivedContext
        {
            User = user,
            Partner = partner,
            Children = children,
            Household = household
        };
    }

    #region Facts
    private static HouseholdFacts BuildHouseholdFacts(
        EntitlementRequest request,
        PersonFacts user,
        PersonFacts? partner)
    {
        return new HouseholdFacts
        {
            HasPartner = request.Household.HasPartner,

            ReceivesUniversalCredit =
                user.ReceivesUniversalCredit ||
                partner?.ReceivesUniversalCredit == true,

            HasAccessToPublicFunds =
                HasAccessToPublicFunds(request.User) ||
                request.Partner is not null &&
                HasAccessToPublicFunds(request.Partner),

            LivesInGreatBritain =
                request.Household.CountryOfResidence is
                    CountryOfResidence.England
                    or CountryOfResidence.Scotland
                    or CountryOfResidence.Wales
        };
    }

    private static PersonFacts BuildPersonFacts(PersonDto person)
    {
        return new PersonFacts
        {
            IsInPaidWork = person.IsInPaidWork == true,
            ReceivesUniversalCredit = person.Benefits.Contains(PersonBenefit.UniversalCredit)
        };
    }

    private static ChildFacts BuildChildFacts(ChildDto child, DateOnly today)
    {
        int? age = child.DateOfBirth is null
            ? null
            : CalculateAgeInYears(child.DateOfBirth.Value, today);

        return new ChildFacts
        {
            Name = child.Name,
            IsBorn = child.BirthStatus == BirthStatus.Born,
            DateOfBirth = child.DateOfBirth,
            DueDate = child.DueDate,
            AgeInYears = age
        };
    }
    #endregion

    #region Helpers
    private static bool HasAccessToPublicFunds(PersonDto person)
    {
        return person.Nationality == Nationality.BritishOrIrishCitizen
               || person.HasAccessToPublicFunds == true
               || person.HasSettledOrPreSettledStatus == true;
    }

    private static int CalculateAgeInYears(DateOnly dateOfBirth, DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;

        if (today < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age;
    }
    #endregion

}