using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("div", Attributes = "data-gov-form-error")]
[HtmlTargetElement("p", Attributes = "data-gov-form-error")]
public class GovFormErrorTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("data-gov-outcome")]
    public string Outcome { get; set; } = string.Empty;

    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Outcome))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'data-gov-form-error' requires 'data-gov-outcome'.");
        }

        return new FormErrorEvent
        {
            ToolName = ToolName,
            Section = Section,
            Outcome = Outcome,
            Action = string.IsNullOrWhiteSpace(Action) ? "error" : Action,
            UiText = string.IsNullOrWhiteSpace(UiText) ? "Continue" : UiText,
            Type = string.IsNullOrWhiteSpace(Type) ? "contact" : Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
    }
}
