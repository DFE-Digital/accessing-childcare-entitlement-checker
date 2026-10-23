using System.Text.Json;
using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers;

public abstract class GovTagHelperBase : TagHelper
{
    [EditorRequired]
    [HtmlAttributeName("data-gov-tool-name")]
    public string ToolName { get; set; } = string.Empty;

    [EditorRequired]
    [HtmlAttributeName("data-gov-section")]
    public string Section { get; set; } = string.Empty;

    [HtmlAttributeName("data-gov-action")]
    public string Action { get; set; } = string.Empty;

    [HtmlAttributeName("data-gov-ui-text")]
    public string UiText { get; set; } = string.Empty;

    [HtmlAttributeName("data-gov-type")]
    public string Type { get; set; } = string.Empty;

    [HtmlAttributeName("data-gov-metric-name")]
    public string? MetricName { get; set; }

    [HtmlAttributeName("data-gov-metric-value")]
    public object? MetricValue { get; set; }

    protected abstract GovEventPayload ValidateAndCreatePayload();

    private static readonly JsonSerializerOptions
        _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(Section))
        {
            throw new InvalidOperationException(
                $"'{context.AllAttributes[0].Name}' requires 'data-gov-section'.");
        }

        var payload = ValidateAndCreatePayload();

        output.Attributes.SetAttribute("data-gov-event", JsonSerializer.Serialize(payload, _jsonOptions));

        CleanupDom(output);
    }

    private static void CleanupDom(TagHelperOutput output)
    {
        // Clean up helper attributes so they aren't rendered into the raw DOM
        output.Attributes.RemoveAll("data-gov-select-content");
        output.Attributes.RemoveAll("data-gov-form-response");
        output.Attributes.RemoveAll("data-gov-form-error");
        output.Attributes.RemoveAll("data-gov-tool-name");
        output.Attributes.RemoveAll("data-gov-section");
        output.Attributes.RemoveAll("data-gov-action");
        output.Attributes.RemoveAll("data-gov-ui-text");
        output.Attributes.RemoveAll("data-gov-type");
        output.Attributes.RemoveAll("data-gov-outcome");
        output.Attributes.RemoveAll("data-gov-response");
        output.Attributes.RemoveAll("data-gov-metric-name");
        output.Attributes.RemoveAll("data-gov-metric-value");
    }

    protected class GovAnalyticsMissingParamaterException(string message) : InvalidOperationException( "[GovAnalytics] " + message);
}
