using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.Encodings.Web;

namespace AccessingChildcareEntitlementChecker.Web.TagHelpers;

[HtmlTargetElement("app-checkboxes", TagStructure = TagStructure.WithoutEndTag)]
public class AppCheckboxesTagHelper(IComponentGenerator componentGenerator)
    : TagHelper
{
    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; } = null!;

    [HtmlAttributeName("exclusive-value")]
    public object? ExclusiveValue { get; set; }

    [HtmlAttributeName("legend")]
    public IHtmlContent? Legend { get; set; }

    [HtmlAttributeName("hint")]
    public string? Hint { get; set; }

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var modelType = For.Metadata.ModelType;
        var isEnumList = modelType.IsGenericType
            && modelType.GetGenericTypeDefinition() == typeof(List<>)
            && modelType.GetGenericArguments()[0].IsEnum;

        if (!isEnumList)
        {
            throw new InvalidOperationException($"The '{For.Name}' expression must be a List of enum values.");
        }

        var fieldName = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
        var idPrefix = TagBuilder.CreateSanitizedId(fieldName, "_");

        var legendHtml = BuildLegendHtml();
        var items = BuildCheckboxItems(fieldName, modelType);
        var errorMessageOptions = BuildError(fieldName, idPrefix);
        var text = For.Metadata.DisplayName ?? For.Name;
        var hint = Hint is null ? null : new HintOptions { Text = Hint };
        var component = await componentGenerator.GenerateCheckboxesAsync(new CheckboxesOptions
        {
            IdPrefix = idPrefix,
            Name = fieldName,
            Fieldset = new FieldsetOptions
            {
                Legend = new FieldsetOptionsLegend
                {
                    Text = text,
                    Html = legendHtml,
                    Classes = "govuk-fieldset__legend--l",
                    IsPageHeading = true
                },
            },
            Hint = hint,
            ErrorMessage = errorMessageOptions,
            Items = items
        });

        component.ApplyToTagHelper(output);
    }

    private ErrorMessageOptions? BuildError(string fieldName, string idPrefix)
    {
        var error = ViewContext.ModelState[fieldName]?.Errors.FirstOrDefault()?.ErrorMessage;
        if (!string.IsNullOrEmpty(error))
        {
            ViewContext.HttpContext.AddPageError(error, $"#{idPrefix}");
            return new ErrorMessageOptions { Text = error };
        }

        return null;
    }

    private List<CheckboxesOptionsItem> BuildCheckboxItems(string fieldName, Type modelType)
    {
        var enumType = modelType.GetGenericArguments()[0];
        var selected = GetSelectedValues(fieldName, ((IEnumerable)For.Model).Cast<Enum>());
        var exclusiveValueText = ExclusiveValue is null
            ? null
            : Convert.ToUInt64(ExclusiveValue).ToString();

        var items = new List<CheckboxesOptionsItem>();

        foreach (Enum value in Enum.GetValues(enumType))
        {
            var valueText = Convert.ToUInt64(value).ToString();
            var display = enumType.GetField(value.ToString())?.GetCustomAttribute<DisplayAttribute>();
            var isExclusive = valueText == exclusiveValueText;
            var hint = display?.GetDescription();

            if (isExclusive)
            {
                items.Add(new CheckboxesOptionsItem { Divider = "or" });
            }

            items.Add(new CheckboxesOptionsItem
            {
                Text = display?.GetName() ?? value.ToString(),
                Value = valueText,
                Checked = selected.Contains(valueText),
                Hint = hint is null ? null : new HintOptions { Text = hint },
                Behaviour = isExclusive ? "exclusive" : null,
                Label = new LabelOptions()
            });
        }

        return items;
    }

    private TemplateString? BuildLegendHtml()
    {
        if (Legend == null)
        {
            return null;
        }

        using var writer = new StringWriter(CultureInfo.CurrentCulture);
        Legend.WriteTo(writer, HtmlEncoder.Default);
        return new TemplateString(writer.ToString());
    }

    private HashSet<string> GetSelectedValues(string fieldName, IEnumerable<Enum> model)
    {
        if (ViewContext.ModelState.TryGetValue(fieldName, out var entry))
        {
            return entry.RawValue switch
            {
                null => [],
                string value => [value],
                string[] values => values.ToHashSet(StringComparer.Ordinal),
                _ => throw new InvalidOperationException($"Unexpected model state value type for '{fieldName}'.")
            };
        }

        return model
            .Select(v => Convert.ToUInt64(v).ToString())
            .ToHashSet(StringComparer.Ordinal);
    }
}