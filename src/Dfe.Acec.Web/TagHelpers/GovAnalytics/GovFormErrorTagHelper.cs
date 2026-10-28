using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("div", Attributes = "govg4-form-error")]
[HtmlTargetElement("p", Attributes = "govg4-form-error")]
public class GovFormErrorTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("govg4-outcome")]
    public string Outcome { get; set; } = string.Empty;

    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Outcome))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'govg4-form-error' requires 'govg4-outcome'.");
        }

        return new FormErrorEvent
        {
            ToolName = ToolName,
            Section = Section,
            Outcome = Outcome,
            Action = string.IsNullOrWhiteSpace(Action) ? "error" : Action,
            UiText = string.IsNullOrWhiteSpace(UiText) ? "Continue" : UiText,
            Type = string.IsNullOrWhiteSpace(Type) ? "form" : Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
    }
}
