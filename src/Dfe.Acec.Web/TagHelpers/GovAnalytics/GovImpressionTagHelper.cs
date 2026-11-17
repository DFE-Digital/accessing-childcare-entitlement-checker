using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("h1", Attributes = "govg4-impression")]
[HtmlTargetElement("h2", Attributes = "govg4-impression")]
[HtmlTargetElement("h3", Attributes = "govg4-impression")]
public class GovImpressionTagHelper : GovTagHelperBase
{
    protected override GovEventPayload CreatePayload() =>
        new ImpressionEvent()
        {
            ToolName = ToolName,
            Section = Section,
            Action = Action,
            UiText = UiText,
            Type =  Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
}
