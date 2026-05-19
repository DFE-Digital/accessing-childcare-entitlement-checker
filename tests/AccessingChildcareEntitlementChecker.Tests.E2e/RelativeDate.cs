using System.Globalization;

namespace AccessingChildcareEntitlementChecker.Tests.E2e;

internal static class RelativeDate
{
    private static readonly CultureInfo GbCulture = CultureInfo.GetCultureInfo("en-GB");

    public static DateOnly Parse(string value)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return value.Trim().ToLowerInvariant() switch
        {
            "today" => today,
            "yesterday" => today.AddDays(-1),
            "tomorrow" => today.AddDays(1),
            _ => DateOnly.Parse(value, GbCulture)
        };
    }

    public static bool IsRelative(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            "today" or "yesterday" or "tomorrow" => true,
            _ => false
        };
    }
}
