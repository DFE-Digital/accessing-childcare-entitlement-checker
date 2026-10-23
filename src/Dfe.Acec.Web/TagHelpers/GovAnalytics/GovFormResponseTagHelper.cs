using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;


[HtmlTargetElement("button", Attributes = "gov-form-response")]
public class GovFormResponseTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("gov-response")]
    public string Response { get; set; } = string.Empty;

    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Response))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'gov-form-response' requires 'gov-response'.");
        }

        return new FormResponseEvent
        {
            ToolName = ToolName,
            Section = Section,
            Response = Response,
            Action = string.IsNullOrWhiteSpace(Action) ? "submit" : Action,
            UiText = string.IsNullOrWhiteSpace(UiText) ? "Continue" : UiText,
            Type = string.IsNullOrWhiteSpace(Type) ? "contact" : Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
    }
}
