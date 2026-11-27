using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("h1", Attributes = GovG4EventAttributes.Impression)]
[HtmlTargetElement("h2", Attributes = GovG4EventAttributes.Impression)]
[HtmlTargetElement("h3", Attributes = GovG4EventAttributes.Impression)]
public class GovImpressionTagHelper : GovTagHelperBase
{
    protected override GovEventPayload CreatePayload() =>
        new ImpressionEvent()
        {
            ToolName = ToolName,
            Section = Section,
            Action = Action,
            UiText = UiText,
            Type = Type,
            MetricName = MetricName,
            MetricValue = MetricValue
        };
}
