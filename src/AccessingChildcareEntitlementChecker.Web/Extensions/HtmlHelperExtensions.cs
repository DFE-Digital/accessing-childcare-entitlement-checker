using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

public static class HtmlHelperExtensions
{
    /// <remarks>
    /// Could potentially be replaced with a custom tag helper if the team can agree on it. See AC-605.
    /// Because of that, we won't bother unit testing this method - the radio buttons are asserted by the UI tests anyway.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public static string EnumDisplayName<TEnum>(this IHtmlHelper html, TEnum value)
        where TEnum : struct, Enum
    {
        var metadataProvider = html.ViewContext.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>();
        var enumMetadata = metadataProvider.GetMetadataForType(typeof(TEnum));
        var numericValue = Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
        var displayName = enumMetadata.EnumGroupedDisplayNamesAndValues?
            .FirstOrDefault(kvp => kvp.Value == numericValue)
            .Key
            .Name;

        return displayName ?? value.ToString();
    }
}
