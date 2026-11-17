using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers.GovAnalytics;

[HtmlTargetElement("a", Attributes = "govg4-navigation")]
public class GovNavigationTagHelper : GovTagHelperBase
{
    protected override string ResolveDefaultType(string tagName) => "link";


    protected override GovEventPayload CreatePayload() =>
        new NavigationEvent()
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
