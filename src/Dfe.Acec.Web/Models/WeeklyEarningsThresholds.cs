using System.Diagnostics;

namespace Dfe.Acec.Web.Models;

public record WeeklyEarningsThresholds(string PerWeek, string PerMonth, string PerYear)
{
    private static readonly WeeklyEarningsThresholds _under18OrApprentice = new("128", "554", "6,656");
    private static readonly WeeklyEarningsThresholds _eighteenToTwenty = new("173", "752", "9,027");
    private static readonly WeeklyEarningsThresholds _twentyOneOrOver = new("203", "879", "10,574");

    public static WeeklyEarningsThresholds Create(AgeRange? age, List<WorkStatusOption> workStatus)
    {
        if (age == null || workStatus.Count == 0)
        {
            throw new InvalidOperationException($"Cannot create {nameof(WeeklyEarningsThresholds)} because the user's age or work status is not set");
        }

        if (workStatus.Contains(WorkStatusOption.Apprentice))
        {
            return _under18OrApprentice;
        }
        else
        {
            return age switch
            {
                AgeRange.UnderEighteen => _under18OrApprentice,
                AgeRange.EighteenToTwenty => _eighteenToTwenty,
                AgeRange.TwentyOneOrOver => _twentyOneOrOver,
                _ => throw new UnreachableException($"Unexpected age range: {age}")
            };
        }
    }
}
