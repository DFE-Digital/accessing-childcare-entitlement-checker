using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;


[HtmlTargetElement("button", Attributes = "data-gov-form-response")]
public class GovFormResponseTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("data-gov-response")]
    public string Response { get; set; } = string.Empty;

    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Response))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'data-gov-form-response' requires 'data-gov-response'.");
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
