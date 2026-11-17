using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;


[HtmlTargetElement("button", Attributes = "govg4-form-response")]
public class GovFormResponseTagHelper : GovTagHelperBase
{
    [EditorRequired]
    [HtmlAttributeName("govg4-response")]
    public string Response { get; set; } = string.Empty;

    protected override void Validate()
    {
        base.Validate();
        if (string.IsNullOrWhiteSpace(Response))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'govg4-form-response' requires 'govg4-response'.");
        }
    }

    protected override GovEventPayload CreatePayload() =>
        new FormResponseEvent
        {
            ToolName = ToolName,
            Section = Section,
            Response = Response,
            Action = Action,
            UiText = UiText,
            Type = Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
}
