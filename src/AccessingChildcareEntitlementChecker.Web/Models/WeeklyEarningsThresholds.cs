using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Diagnostics;

public record WeeklyEarningsThresholds(string PerWeek, string PerMonth, string PerYear)
{
    public static readonly WeeklyEarningsThresholds Under18OrApprentice = new("128", "554", "6,656");
    public static readonly WeeklyEarningsThresholds EighteenToTwenty = new("174", "752", "9,027");
    public static readonly WeeklyEarningsThresholds TwentyOneOrOver = new("203", "879", "10,574");

    public static WeeklyEarningsThresholds Factory(AgeRange? age, List<WorkStatusOption> workStatus)
    {
        if (age == null || workStatus.Count == 0)
        {
            throw new InvalidOperationException($"Cannot create {nameof(WeeklyEarningsThresholds)} because the user's age or work status is not set");
        }

        if (workStatus.Contains(WorkStatusOption.Apprentice))
        {
            return Under18OrApprentice;
        }
        else
        {
            return age switch
            {
                AgeRange.UnderEighteen => Under18OrApprentice,
                AgeRange.EighteenToTwenty => EighteenToTwenty,
                AgeRange.TwentyOneOrOver => TwentyOneOrOver,
                _ => throw new UnreachableException($"Unexpected age range: {age}")
            };
        }
    }
}
