using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dfe.Acec.Web.TagHelpers;
using Dfe.Acec.Web.TagHelpers.GovAnalytics;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.Tests.Unit.TagHelpers;

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores")]
public class GovAnalyticsTagHelperTests
{
    private static (TagHelperContext context, TagHelperOutput output) CreateTagHelperContextAndOutput(
        string tagName,
        TagHelperAttributeList attributes,
        string innerText = "Default Button Text")
    {
        var context = new TagHelperContext(
            attributes,
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var tagHelperContent = new DefaultTagHelperContent();
        tagHelperContent.SetContent(innerText);

        var output = new TagHelperOutput(
            tagName,
            attributes,
            getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(tagHelperContent));

        return (context, output);
    }

    [Fact]
    public async Task SelectContentTagHelper_ThrowsException_WhenRequiredAttributesAreMissing()
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
            { "govg4-select-content", null },
            { "govg4-tool-name", "childcare_eligibility_checker" },
            { "govg4-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.ProcessAsync(context, output));
        Assert.Contains("govg4-select-content", exception.Message);
    }


    [Fact]
    public async Task FormResponseTagHelper_ThrowsException_WhenResponseIsMissing()
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
            { "govg4-form-response", null },
            { "govg4-tool-name", "childcare_eligibility_checker" },
            { "govg4-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        await Assert.ThrowsAsync<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.ProcessAsync(context, output));
    }

    [Fact]
    public async Task FormErrorTagHelper_ThrowsException_WhenResponseIsMissing()
    {
        // Arrange
        var tagHelper = new GovFormErrorTagHelper()
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details"
            // Outcome omitted
        };

        var attributes = new TagHelperAttributeList
        {
            { "govg4-form-response", null },
            { "govg4-tool-name", "childcare_eligibility_checker" },
            { "govg4-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        await Assert.ThrowsAsync<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.ProcessAsync(context, output));
    }

    private static (GovTagHelperBase concrete, TagHelperAttributeList inputAttributes, Dictionary<string, string> expectedProperties)
        GetExpectedAttributes(Type type)
    {
        if (Activator.CreateInstance(type) is not GovTagHelperBase expectedModel)
        {
            throw new ArgumentException($"Unsupported TagHelper type: {type.Name}, unable to cast to GovTagHelperBase");
        }
        // base model defaults
        expectedModel.ToolName = "checker";
        expectedModel.Section = "details";
        expectedModel.UiText = "Some Ui Text";
        expectedModel.MetricName = "count";
        expectedModel.MetricValue = 2;

        string? expectedEventName = null;
        string? markerAttributeName = null;

        // Type-specific model, markers, names
        switch (expectedModel)
        {
            case GovSelectContentTagHelper select:
                select.Action = "Add";
                select.Type = "button";
                expectedEventName = "select_content";
                markerAttributeName = "govg4-select-content";
                break;

            case GovFormResponseTagHelper response:
                response.Action = "submit";
                response.Type = "form";
                response.Response = "a response";
                expectedEventName = "form_response";
                markerAttributeName = "govg4-form-response";
                break;

            case GovFormErrorTagHelper error:
                error.Action = "error";
                error.Type = "form";
                error.Outcome = "an outcome message";
                expectedEventName = "form_error";
                markerAttributeName = "govg4-form-error";
                break;

            default:
                throw new ArgumentException($"Unsupported TagHelper type: {type.Name}");
        }

        //base attributes and expected properties
        var inputAttributes = new TagHelperAttributeList
        {
            { markerAttributeName, null },
            { "govg4-tool-name", expectedModel.ToolName },
            { "govg4-section", expectedModel.Section },
            { "govg4-action", expectedModel.Action },
            { "govg4-ui-text", expectedModel.UiText },
            { "govg4-metric-name", expectedModel.MetricName },
            { "govg4-metric-value", expectedModel.MetricValue }
        };


        var expectedPropertyMap = new Dictionary<string, string>
        {
            { "event", "gov_event" },
            { "event_name", expectedEventName! },
            { "tool_name", expectedModel.ToolName },
            { "section", expectedModel.Section },
            { "action", expectedModel.Action },
            { "ui_text", expectedModel.UiText },
            { "metric_name", expectedModel.MetricName },
           // { "metric_value", expectedModel.MetricValue.ToString()! }
        };

        // type specific attributes and expected properties
        switch (expectedModel)
        {

            case GovFormResponseTagHelper respModel:
                inputAttributes.Add("govg4-response", respModel.Response);
                expectedPropertyMap.Add("response", respModel.Response);
                break;
            case GovFormErrorTagHelper errModel:
                inputAttributes.Add("govg4-outcome", errModel.Outcome);
                expectedPropertyMap.Add("outcome", errModel.Outcome);
                break;
        }

        return (expectedModel, inputAttributes, expectedPropertyMap);
    }

    [Theory]
    [InlineData(typeof(GovSelectContentTagHelper))]
    [InlineData(typeof(GovFormResponseTagHelper))]
    [InlineData(typeof(GovFormErrorTagHelper))]
    public async Task TagHelperSerializesCorrectDataGovEventAndCleansUpAttributes(Type type)
    {
        // Arrange
        var (concrete, inputAttributes, expectedPropertyMap) = GetExpectedAttributes(type);
        var expectedRemovedTags = new TagHelperAttributeList(inputAttributes);
        var (context, output) = CreateTagHelperContextAndOutput("button", inputAttributes);

        // Act
        await concrete.ProcessAsync(context, output);

        // Assert
        // verify data-govg4-event payload structure
        var jsonAttribute = output.Attributes["data-govg4-event"];
        Assert.NotNull(jsonAttribute);

        using var jsonDoc = JsonDocument.Parse(jsonAttribute.Value.ToString()!);
        var jsonModel = jsonDoc.RootElement;

        foreach (var (propertyName, expectedValue) in expectedPropertyMap)
        {
            Assert.True(
                jsonModel.TryGetProperty(propertyName, out var propertyElement),
                $"JSON payload on {type.Name} is missing the expected property '{propertyName}'."
            );

            var actualValue = propertyElement.GetString();

            Assert.True(
                expectedValue == actualValue,
                $"JSON property '{propertyName}' mismatch on {type.Name}. Expected '{expectedValue}', but found '{actualValue}'."
            );
        }

        // verify govg4-* attributes were stripped
        foreach (var attr in expectedRemovedTags)
        {
            var attributeName = attr.Name;
            var actualAttribute = output.Attributes[attributeName];

            Assert.True(
                actualAttribute is null,
                $"Expected temporary attribute '{attributeName}' to be stripped from output, but it was retained."
            );
        }
    }

    [Fact]
    public async Task Process_OmittedUiText_FallsBackToInnerHtml()
    {
        // Arrange: UiText is intentionally left null
        var tagHelper = new GovSelectContentTagHelper
        {
            ToolName = "childcare_eligibility_checker",
            Section = "applicant_details",
            Action = "continue"
        };

        var inputAttributes = new TagHelperAttributeList
        {
            { "govg4-select-content", null },
            { "govg4-tool-name", tagHelper.ToolName },
            { "govg4-section", tagHelper.Section },
            { "govg4-action", tagHelper.Action }
        };

        const string expectedButtonText = "Save and continue";
        var (context, output) = CreateTagHelperContextAndOutput(
            "button",
            inputAttributes,
            innerText: expectedButtonText
        );

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var jsonAttr = output.Attributes["data-govg4-event"]?.Value?.ToString();
        Assert.True(
            jsonAttr is not null,
            "Expected 'data-govg4-event' attribute to be emitted."
        );

        using var doc = JsonDocument.Parse(jsonAttr!);
        var actualUiText = doc.RootElement.GetProperty("ui_text").GetString();

        Assert.True(
            expectedButtonText == actualUiText,
            $"ui_text fallback mismatch. Expected '{expectedButtonText}', but found '{actualUiText}'."
        );
    }

}
