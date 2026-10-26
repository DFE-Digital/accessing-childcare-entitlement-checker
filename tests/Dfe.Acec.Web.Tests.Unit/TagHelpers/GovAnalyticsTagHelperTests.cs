using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Dfe.Acec.Web.TagHelpers;
using Dfe.Acec.Web.TagHelpers.GovAnalytics;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NSubstitute.Exceptions;

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
        var exception = Assert.Throws<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.Process(context, output));
        Assert.Contains("gov-select-content", exception.Message);
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
        Assert.Throws<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.Process(context, output));
    }

    [Fact]
    public void FormErrorTagHelper_ThrowsException_WhenResponseIsMissing()
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
            { "gov-form-response", null },
            { "gov-tool-name", "childcare_eligibility_checker" },
            { "gov-section", "children details" }
        };

        var (context, output) = CreateTagHelperContextAndOutput("button", attributes);

        // Act & Assert
        Assert.Throws<GovTagHelperBase.GovAnalyticsMissingParamaterException>(() => tagHelper.Process(context, output));
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
                markerAttributeName = "gov-select-content";
                break;

            case GovFormResponseTagHelper response:
                response.Action = "submit";
                response.Type = "form";
                response.Response = "a response";
                expectedEventName = "form_response";
                markerAttributeName = "gov-form-response";
                break;

            case GovFormErrorTagHelper error:
                error.Action = "error";
                error.Type = "form";
                error.Outcome = "an outcome message";
                expectedEventName = "form_error";
                markerAttributeName = "gov-form-error";
                break;

            default:
                throw new ArgumentException($"Unsupported TagHelper type: {type.Name}");
        }

        //base attributes and expected properties
        var inputAttributes = new TagHelperAttributeList
        {
            { markerAttributeName, null },
            { "gov-tool-name", expectedModel.ToolName },
            { "gov-section", expectedModel.Section },
            { "gov-action", expectedModel.Action },
            { "gov-ui-text", expectedModel.UiText },
            { "gov-metric-name", expectedModel.MetricName },
            { "gov-metric-value", expectedModel.MetricValue }
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
                inputAttributes.Add("gov-response", respModel.Response);
                expectedPropertyMap.Add("response", respModel.Response);
                break;
            case GovFormErrorTagHelper errModel:
                inputAttributes.Add("gov-outcome", errModel.Outcome);
                expectedPropertyMap.Add("outcome", errModel.Outcome);
                break;
        }

        return (expectedModel, inputAttributes, expectedPropertyMap);
    }

    [Theory]
    [InlineData(typeof(GovSelectContentTagHelper))]
    [InlineData(typeof(GovFormResponseTagHelper))]
    [InlineData(typeof(GovFormErrorTagHelper))]
    public void TagHelperSerializesCorrectDataGovEventAndCleansUpAttributes(Type type)
    {
        // Arrange
        var (concrete, inputAttributes, expectedPropertyMap) = GetExpectedAttributes(type);
        var expectedRemovedTags = new TagHelperAttributeList(inputAttributes);
        var (context, output) = CreateTagHelperContextAndOutput("button", inputAttributes);

        // Act
        concrete.Process(context, output);

        // Assert
        // verify data-gov-event payload structure
        var jsonAttribute = output.Attributes["data-gov-event"];
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

        // verify gov-* attributes were stripped
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

}
