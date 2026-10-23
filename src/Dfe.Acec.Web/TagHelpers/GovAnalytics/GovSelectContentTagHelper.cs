using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;


[HtmlTargetElement("button", Attributes = "gov-select-content")]
[HtmlTargetElement("a", Attributes = "gov-select-content")]
public class GovSelectContentTagHelper : GovTagHelperBase
{
    protected override GovEventPayload ValidateAndCreatePayload()
    {
        if (string.IsNullOrWhiteSpace(Action) || string.IsNullOrWhiteSpace(UiText))
        {
            throw new GovAnalyticsMissingParamaterException(
                "'gov-select-content' requires both 'gov-action' and 'gov-ui-text'.");
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
