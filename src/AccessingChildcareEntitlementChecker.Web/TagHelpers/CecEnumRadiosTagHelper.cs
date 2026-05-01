using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AccessingChildcareEntitlementChecker.Web.TagHelpers;

[HtmlTargetElement("cec-enum-radios", TagStructure = TagStructure.WithoutEndTag)]
public class CecEnumRadiosTagHelper : TagHelper
{
    private readonly IComponentGenerator _componentGenerator;
    private readonly IModelMetadataProvider _metadataProvider;

    public CecEnumRadiosTagHelper(IComponentGenerator componentGenerator, IModelMetadataProvider metadataProvider)
    {
        _componentGenerator = componentGenerator;
        _metadataProvider = metadataProvider;
    }

    [HtmlAttributeName("for")]
    public ModelExpression For { get; set; } = null!;

    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext ViewContext { get; set; } = null!;

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var enumType = Nullable.GetUnderlyingType(For.Metadata.ModelType) ?? For.Metadata.ModelType;
        var fieldName = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
        var idPrefix = fieldName.Replace('.', '_');
        var currentValue = For.Model?.ToString();

        var enumMetadata = _metadataProvider.GetMetadataForType(enumType);
        var numericToLabel = enumMetadata.EnumGroupedDisplayNamesAndValues!
            .ToDictionary(kvp => kvp.Value, kvp => kvp.Key.Name ?? kvp.Value);

        var items = Enum.GetValues(enumType)
            .Cast<object>()
            .Select(value =>
            {
                var memberName = value.ToString()!;
                var numericStr = Convert.ToInt64(value).ToString();
                return new RadiosOptionsItem
                {
                    Text = numericToLabel.GetValueOrDefault(numericStr, memberName),
                    Value = memberName,
                    Checked = currentValue == memberName,
                    Label = new LabelOptions()
                };
            })
            .ToList();

        var errors = ViewContext.ModelState[fieldName]?.Errors;

        var component = await _componentGenerator.GenerateRadiosAsync(new RadiosOptions
        {
            IdPrefix = idPrefix,
            Name = fieldName,
            Fieldset = new FieldsetOptions
            {
                Legend = new FieldsetOptionsLegend
                {
                    Text = For.Metadata.DisplayName ?? For.Name,
                    IsPageHeading = true,
                    Classes = "govuk-fieldset__legend--l"
                }
            },
            Hint = For.Metadata.Description is { } desc
                ? new HintOptions { Text = desc }
                : null,
            ErrorMessage = errors?.Count > 0
                ? new ErrorMessageOptions { Text = errors[0].ErrorMessage }
                : null,
            Items = items
        });

        component.ApplyToTagHelper(output);
    }
}