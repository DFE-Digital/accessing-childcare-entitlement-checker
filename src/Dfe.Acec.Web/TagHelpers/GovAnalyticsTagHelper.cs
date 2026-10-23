using System.Text.Json;
using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers;

public abstract class GovTagHelperBase : TagHelper
{
    [EditorRequired]
    [HtmlAttributeName("gov-tool-name")]
    public string ToolName { get; set; } = string.Empty;

    [EditorRequired]
    [HtmlAttributeName("gov-section")]
    public string Section { get; set; } = string.Empty;

    [HtmlAttributeName("gov-action")]
    public string Action { get; set; } = string.Empty;

    [HtmlAttributeName("gov-ui-text")]
    public string UiText { get; set; } = string.Empty;

    [HtmlAttributeName("gov-type")]
    public string Type { get; set; } = string.Empty;

    [HtmlAttributeName("gov-metric-name")]
    public string? MetricName { get; set; }

    [HtmlAttributeName("gov-metric-value")]
    public object? MetricValue { get; set; }

    protected abstract GovEventPayload ValidateAndCreatePayload();

    private static readonly JsonSerializerOptions
        _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Section))
        {
            throw new InvalidOperationException(
                $"'{context.AllAttributes[0].Name}' requires 'gov-section'.");
        }

        var payload = ValidateAndCreatePayload();

        output.Attributes.SetAttribute("data-gov-event", JsonSerializer.Serialize(payload, _jsonOptions));

        CleanupDom(output);
    }

    private static void CleanupDom(TagHelperOutput output)
    {
        // Clean up helper attributes so they aren't rendered into the raw DOM
        output.Attributes.RemoveAll("gov-select-content");
        output.Attributes.RemoveAll("gov-form-response");
        output.Attributes.RemoveAll("gov-form-error");
        output.Attributes.RemoveAll("gov-tool-name");
        output.Attributes.RemoveAll("gov-section");
        output.Attributes.RemoveAll("gov-action");
        output.Attributes.RemoveAll("gov-ui-text");
        output.Attributes.RemoveAll("gov-type");
        output.Attributes.RemoveAll("gov-outcome");
        output.Attributes.RemoveAll("gov-response");
        output.Attributes.RemoveAll("gov-metric-name");
        output.Attributes.RemoveAll("gov-metric-value");
    }

    protected class GovAnalyticsMissingParamaterException(string message) : InvalidOperationException( "[GovAnalytics] " + message);
}
