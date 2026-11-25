using System.Text.Json;
using Dfe.Acec.Web.Extensions;
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
    public int? MetricValue { get; set; }

    private string _attributeName = "govg4";

    protected abstract GovEventPayload CreatePayload();

    private static readonly JsonSerializerOptions
        _jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        _attributeName = context.AllAttributes.FirstOrDefault()?.Name ?? _attributeName;
        await ResolveDefaultsAsync(context, output);
        Validate();
        var payload = CreatePayload();
        var json = JsonSerializer.Serialize(payload, payload.GetType(), _jsonOptions);
        output.Attributes.SetAttribute("data-govg4-event", json);

        CleanupDom(output);
    }

    private async Task ResolveDefaultsAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(UiText))
        {
            var childContent = await output.GetChildContentAsync();
            UiText = System.Net.WebUtility.HtmlDecode(childContent.GetContent()).Trim();

            if (UiText.ContainsHtml())
            {
                throw new GovAnalyticsMissingParamaterException(
                    $"'{_attributeName}' inferred 'govg4-ui-text' from child content containing HTML ('{UiText}'). Please specify 'govg4-ui-text' explicitly.");
            }
        }

        if (string.IsNullOrWhiteSpace(Type))
        {
            Type = ResolveDefaultType(context.TagName);
        }
    }

    protected virtual void Validate()
    {

        if (string.IsNullOrWhiteSpace(Section))
        {
            throw new GovAnalyticsMissingParamaterException(
                $"'{_attributeName}' requires 'govg4-section'.");
        }

        if (string.IsNullOrWhiteSpace(Action))
        {
            throw new GovAnalyticsMissingParamaterException(
                "Analytics attribute requires 'govg4-action' and 'govg4-ui-text'");
        }

        var hasMetricName = !string.IsNullOrWhiteSpace(MetricName);
        var hasMetricValue = MetricValue is not null;

        if (hasMetricName && !hasMetricValue)
        {
            throw new GovAnalyticsMissingParamaterException(
                "When setting 'govg4-metric-name', you must also provide 'govg4-metric-value'.");
        }
        if (!hasMetricName && hasMetricValue)
        {
            throw new GovAnalyticsMissingParamaterException(
                "When setting 'govg4-metric-value', you must also provide 'govg4-metric-name'.");
        }
    }

    protected virtual string ResolveDefaultType(string tagName) => tagName?.ToLowerInvariant() ?? "none";


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

    public class GovAnalyticsMissingParamaterException(string message) : InvalidOperationException("[GovAnalytics] " + message);
}
