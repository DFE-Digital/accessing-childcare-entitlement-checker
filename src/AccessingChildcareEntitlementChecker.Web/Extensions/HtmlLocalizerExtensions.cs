using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Extensions;

/// <summary>
/// Extension methods for <see cref="IHtmlLocalizer"/>.
/// </summary>
public static class HtmlLocalizerExtensions
{
    /// <summary>
    /// Localizes the string with the specified key, ensuring the child's name parameter is wrapped in a Microsoft Clarity mask.
    /// </summary>
    /// <param name="localizer">The HTML localizer.</param>
    /// <param name="key">The resource key.</param>
    /// <param name="childName">The child's name to be masked.</param>
    /// <param name="additionalArgs">Additional arguments for the localized string.</param>
    /// <returns>A localized HTML string with the masked child name.</returns>
    public static LocalizedHtmlString MaskChildName(
        this IHtmlLocalizer localizer,
        string key,
        string? childName,
        params object[] additionalArgs)
    {
        var nameToMask = string.IsNullOrWhiteSpace(childName) ? "child" : childName;
        var encodedName = HtmlEncoder.Default.Encode(nameToMask);
        var maskedName = new HtmlString($"<span data-clarity-mask=\"true\">{encodedName}</span>");

        var combinedArgs = new object[additionalArgs.Length + 1];
        combinedArgs[0] = maskedName;
        Array.Copy(additionalArgs, 0, combinedArgs, 1, additionalArgs.Length);

        return localizer[key, combinedArgs];
    }
}
