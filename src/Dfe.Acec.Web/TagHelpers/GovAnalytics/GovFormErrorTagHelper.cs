using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("div", Attributes = GovG4EventAttributes.FormError)]
[HtmlTargetElement("p", Attributes = GovG4EventAttributes.FormError)]
public class GovFormErrorTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("govg4-outcome")]
    public string Outcome { get; set; } = string.Empty;

    protected override void Validate()
    {
        base.Validate();
        if (string.IsNullOrWhiteSpace(Outcome))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'govg4-form-error' requires 'govg4-outcome'.");
        }
    }

    protected override GovEventPayload CreatePayload() =>
        new FormErrorEvent()
        {
            ToolName = ToolName,
            Section = Section,
            Outcome = Outcome,
            Action = Action,
            UiText = UiText,
            Type = Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
}
