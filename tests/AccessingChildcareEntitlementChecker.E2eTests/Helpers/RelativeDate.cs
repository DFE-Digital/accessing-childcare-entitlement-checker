using System.Globalization;

namespace AccessingChildcareEntitlementChecker.E2eTests.Helpers;

internal static class RelativeDate
{
    private const string Today = "today";
    private const string Yesterday = "yesterday";
    private const string Tomorrow = "tomorrow";

    private static readonly CultureInfo GbCulture = CultureInfo.GetCultureInfo("en-GB");

    public static DateOnly Parse(string value)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return value.Trim().ToLowerInvariant() switch
        {
            Today => today,
            Yesterday => today.AddDays(-1),
            Tomorrow => today.AddDays(1),
            _ => DateOnly.Parse(value, GbCulture)
        };
    }

    public static bool IsRelative(string value)
    {
        return value.Trim().ToLowerInvariant() switch
        {
            Today or Yesterday or Tomorrow => true,
            _ => false
        };
    }
}
