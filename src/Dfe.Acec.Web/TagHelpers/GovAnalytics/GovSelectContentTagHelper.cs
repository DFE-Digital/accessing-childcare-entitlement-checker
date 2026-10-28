using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;


[HtmlTargetElement("button", Attributes = "govg4-select-content")]
[HtmlTargetElement("a", Attributes = "govg4-select-content")]
public class GovSelectContentTagHelper : GovTagHelperBase
{
    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Action) || string.IsNullOrWhiteSpace(UiText))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'govg4-select-content' requires both 'govg4-action' and 'govg4-ui-text'.");
        }

        return new SelectContentEvent
        {
            ToolName = ToolName,
            Section = Section,
            Action = Action,
            UiText = UiText,
            Type = string.IsNullOrWhiteSpace(Type) ? "button" : Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
    }
}
