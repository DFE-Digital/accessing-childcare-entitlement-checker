using System.Collections.Frozen;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

public static class GovG4EventAttributes
{
    public const string FormError = "govg4-form-error";
    public const string FormResponse = "govg4-form-response";
    public const string Impression = "govg4-impression";
    public const string Navigation = "govg4-navigation";
    public const string Interaction = "govg4-select-content";

    public static FrozenSet<string> All { get; } = new[]
    {
        FormError,
        FormResponse,
        Impression,
        Navigation,
        Interaction
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
    //chris todo: create a spreadsheet for Jack (and make the ticket, and add column for "PII??"
}
