using System.Globalization;

namespace Dfe.Acec.Web.Tests.E2e.Helpers;

internal static class RelativeDate
{
    private const string _today = "today";
    private const string _yesterday = "yesterday";
    private const string _tomorrow = "tomorrow";

    private static readonly CultureInfo _gbCulture = CultureInfo.GetCultureInfo("en-GB");

    public static DateOnly Parse(string value)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return value.Trim().ToLowerInvariant() switch
        {
            _today => today,
            _yesterday => today.AddDays(-1),
            _tomorrow => today.AddDays(1),
            _ => DateOnly.Parse(value, _gbCulture)
        };
    }

    public static bool IsRelative(string value) => value.Trim().ToLowerInvariant() switch
    {
        _today or _yesterday or _tomorrow => true,
        _ => false
    };
}
