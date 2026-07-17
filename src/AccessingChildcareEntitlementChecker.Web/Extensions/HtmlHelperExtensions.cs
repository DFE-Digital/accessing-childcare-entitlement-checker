using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

public static class HtmlHelperExtensions
{
    /// <remarks>
    /// Could potentially be replaced with a custom tag helper if the team can agree on it. See AC-605.
    /// Because of that, we won't bother unit testing this method - the radio buttons are asserted by the UI tests anyway.
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "May be replaced and covered by UI tests")]
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

    /// <remarks>
    /// Similar to EnumDisplayName.
    /// </remarks>
    [ExcludeFromCodeCoverage(Justification = "May be replaced and covered by UI tests")]
    public static string? EnumDescription<TEnum>(this IHtmlHelper html, TEnum value)
        where TEnum : struct, Enum
    {
        var member = typeof(TEnum)
            .GetMember(value.ToString())
            .SingleOrDefault();

        return member?
            .GetCustomAttribute<DisplayAttribute>()?
            .GetDescription();
    }
}
