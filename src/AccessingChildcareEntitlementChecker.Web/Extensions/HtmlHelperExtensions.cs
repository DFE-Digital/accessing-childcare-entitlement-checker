using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;

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

    public static string? GetDescriptionFor<TModel, TValue>(
        this IHtmlHelper<TModel> html,
        Expression<Func<TModel, TValue>> expression)
    {
        var provider = html.ViewContext.HttpContext.RequestServices
            .GetRequiredService<IModelExpressionProvider>();

        var modelExpression = provider.CreateModelExpression(
            html.ViewData,
            expression);

        return modelExpression.Metadata.Description;
    }
}
