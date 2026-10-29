using System.Text.Json;
using Dfe.Acec.Web.Models.Analytics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.TagHelpers;

public abstract class GovTagHelperBase : TagHelper
{
    [EditorRequired]
    [HtmlAttributeName("govg4-tool-name")]
    public string ToolName { get; set; } = string.Empty;

    [EditorRequired]
    [HtmlAttributeName("govg4-section")]
    public string Section { get; set; } = string.Empty;

    [HtmlAttributeName("govg4-action")]
    public string Action { get; set; } = string.Empty;

    [HtmlAttributeName("govg4-ui-text")]
    public string UiText { get; set; } = string.Empty;

    [HtmlAttributeName("govg4-type")]
    public string Type { get; set; } = string.Empty;

    [HtmlAttributeName("govg4-metric-name")]
    public string? MetricName { get; set; }

    [HtmlAttributeName("govg4-metric-value")]
    public object? MetricValue { get; set; }

    protected abstract GovEventPayload ValidateAndCreatePayload();

    private static readonly JsonSerializerOptions
        _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(UiText))
        {
            var childContent = await output.GetChildContentAsync();
            UiText = childContent.GetContent().Trim();
        }
        if (string.IsNullOrWhiteSpace(Section))
        {
            throw new InvalidOperationException(
                $"'{context.AllAttributes[0].Name}' requires 'govg4-section'.");
        }

        var payload = ValidateAndCreatePayload();
        var json = JsonSerializer.Serialize(payload, payload.GetType(), _jsonOptions);
        output.Attributes.SetAttribute("data-govg4-event", json);

        CleanupDom(output);
    }

    private static void CleanupDom(TagHelperOutput output)
    {
        // Clean up helper attributes so they aren't rendered into the raw DOM
        output.Attributes.RemoveAll("govg4-select-content");
        output.Attributes.RemoveAll("govg4-form-response");
        output.Attributes.RemoveAll("govg4-form-error");
        output.Attributes.RemoveAll("govg4-tool-name");
        output.Attributes.RemoveAll("govg4-section");
        output.Attributes.RemoveAll("govg4-action");
        output.Attributes.RemoveAll("govg4-ui-text");
        output.Attributes.RemoveAll("govg4-type");
        output.Attributes.RemoveAll("govg4-outcome");
        output.Attributes.RemoveAll("govg4-response");
        output.Attributes.RemoveAll("govg4-metric-name");
        output.Attributes.RemoveAll("govg4-metric-value");
    }

    public class GovAnalyticsMissingParamaterException(string message) : InvalidOperationException( "[GovAnalytics] " + message);
}
