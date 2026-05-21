namespace AccessingChildcareEntitlementChecker.RulesEngine.Helpers;

public static class TermDateCalculator
{
    public static DateOnly GetNextTermStartDate(DateOnly date)
    {
        var year = date.Year;

        if (date <= new DateOnly(year, 3, 31))
        {
            return new DateOnly(year, 4, 1);
        }

        if (date <= new DateOnly(year, 8, 31))
        {
            return new DateOnly(year, 9, 1);
        }

        return new DateOnly(year + 1, 1, 1);
    }
}