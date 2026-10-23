using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dfe.Acec.Web.TagHelpers.GovAnalytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.Tests.Unit.TagHelpers;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GovAnalyticsTagHelperTests
{
    private static (TagHelperContext context, TagHelperOutput output) CreateTagHelperContextAndOutput(
        string tagName,
        TagHelperAttributeList attributes)
    {
        var context = new TagHelperContext(
            attributes,
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            tagName,
            attributes,
            getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        return (context, output);
    }

    [Fact]
    public void SelectContentTagHelper_ThrowsException_WhenRequiredAttributesAreMissing()
    {
        // Arrange
        var tagHelper = new GovSelectContentTagHelper
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details"
            // Action and UiText deliberately omitted
        };

        var attributes = new TagHelperAttributeList
        {
            { "gov-select-content", null },
            { "gov-tool-name", "childcare_eligibility_checker" },
            { "gov-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => tagHelper.Process(context, output));
        Assert.Contains("gov-select-content", exception.Message);
    }

    [Fact]
    public void SelectContentTagHelper_SerializesCorrectDataGovEventAndCleansUpAttributes()
    {
        // Arrange
        var tagHelper = new GovSelectContentTagHelper
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "add child",
            UiText = "+ Add Child",
            Type = "button",
            MetricName = "children_count",
            MetricValue = 2
        };

        var attributes = new TagHelperAttributeList
        {
            { "gov-select-content", null },
            { "gov-tool-name", "childcare_eligibility_checker" },
            { "gov-section", "children details" },
            { "gov-action", "add child" },
            { "gov-ui-text", "+ Add Child" },
            { "gov-metric-name", "children_count" },
            { "gov-metric-value", 2 }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act
        tagHelper.Process(context, output);

        // Assert: 1. Verify JSON output
        var jsonAttribute = output.Attributes["data-gov-event"];
        Assert.NotNull(jsonAttribute);

        using var jsonDoc = JsonDocument.Parse(jsonAttribute.Value.ToString()!);
        var root = jsonDoc.RootElement;

        Assert.Equal("gov_event", root.GetProperty("event").GetString());
        Assert.Equal("select_content", root.GetProperty("event_name").GetString());
        Assert.Equal("childcare_eligibility_checker", root.GetProperty("tool_name").GetString());
        Assert.Equal("children details", root.GetProperty("section").GetString());
        Assert.Equal("add child", root.GetProperty("action").GetString());
        Assert.Equal("+ Add Child", root.GetProperty("ui_text").GetString());
        Assert.Equal("children_count", root.GetProperty("metric_name").GetString());
        Assert.Equal(2, root.GetProperty("metric_value").GetInt32());

        // Assert: 2. Verify temporary gov-* attributes were stripped
        Assert.Null(output.Attributes["gov-select-content"]);
        Assert.Null(output.Attributes["gov-tool-name"]);
        Assert.Null(output.Attributes["gov-section"]);
        Assert.Null(output.Attributes["gov-action"]);
        Assert.Null(output.Attributes["gov-ui-text"]);
    }

    [Fact]
    public void FormResponseTagHelper_ThrowsException_WhenResponseIsMissing()
    {
        // Arrange
        var tagHelper = new GovFormResponseTagHelper
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details"
            // Response omitted
        };

        var attributes = new TagHelperAttributeList
        {
            { "gov-form-response", null },
            { "gov-tool-name", "childcare_eligibility_checker" },
            { "gov-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tagHelper.Process(context, output));
    }
}
