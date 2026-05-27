namespace AccessingChildcareEntitlementChecker.RulesEngine.Helpers;

public static class AgeCalculations
{
    public static int CalculateAgeInYears(
        DateOnly dateOfBirth,
        DateOnly today)
    {
        var age = today.Year - dateOfBirth.Year;

        if (today < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age;
    }

    public static int CalculateAgeInMonths(
        DateOnly dateOfBirth,
        DateOnly today)
    {
        var months =
            ((today.Year - dateOfBirth.Year) * 12)
            + today.Month
            - dateOfBirth.Month;

        if (today.Day < dateOfBirth.Day)
        {
            months--;
        }

        return months;
    }
}