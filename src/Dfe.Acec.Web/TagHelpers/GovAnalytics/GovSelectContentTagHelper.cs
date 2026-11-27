using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("button", Attributes = GovG4EventAttributes.Interaction)]
public class GovSelectContentTagHelper : GovTagHelperBase
{
    protected override GovEventPayload CreatePayload() =>
        new SelectContentEvent
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
