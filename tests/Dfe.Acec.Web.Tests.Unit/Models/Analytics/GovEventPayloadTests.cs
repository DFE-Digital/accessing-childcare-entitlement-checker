using System.Text.Json;
using Dfe.Acec.Web.Models.Analytics;

namespace Dfe.Acec.Web.Tests.Unit.Models.Analytics;



public class GovEventPayloadTests
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    [Fact]
    public void ImpressionEventSerializesToExpectedJsonStructure()
    {
        var payload = new ImpressionEvent()
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "add child",
            UiText = "Go to a Child",
            Type = "button"
        };

        AssertSerializesBaseEvent(payload, expectedEventName: "view_item_list", root =>
        {
            Assert.Equal(1, root.GetProperty("metric_value").GetInt32());
            Assert.Equal("impression", root.GetProperty("metric_name").GetString());
        });
    }

    [Fact]
    public void NavigationEventSerializesToExpectedJsonStructure()
    {
        var payload = new NavigationEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "add child",
            UiText = "Go to a Child",
            Type = "button"
        };

        AssertSerializesBaseEvent(payload, expectedEventName: "navigation", root =>
        {
            Assert.False(root.TryGetProperty("metric_name", out _));
            Assert.False(root.TryGetProperty("metric_value", out _));
        });
    }

    [Fact]
    public void SelectContentEventSerializesToExpectedJsonStructure()
    {
        var payload = new SelectContentEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "add child",
            UiText = "Add a Child",
            Type = "button"
        };

        AssertSerializesBaseEvent(payload, expectedEventName: "select_content", root =>
        {
            Assert.False(root.TryGetProperty("metric_name", out _));
            Assert.False(root.TryGetProperty("metric_value", out _));
        });
    }

    [Fact]
    public void FormErrorEventSerializesOutcomeProperty()
    {
        var payload = new FormErrorEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Outcome = "Select if you have a partner",
            Action = "error",
            UiText = "Continue",
            Type = "contact"
        };

        AssertSerializesBaseEvent(payload, expectedEventName: "form_error", root =>
        {
            Assert.Equal("Select if you have a partner", root.GetProperty("outcome").GetString());
        });
    }

    [Fact]
    public void FormResponseEventSerializesResponseProperty()
    {
        var payload = new FormResponseEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "submit",
            UiText = "Continue",
            Type = "contact",
            Response = "Successfully added child"
        };

        AssertSerializesBaseEvent(payload, expectedEventName: "form_response", root =>
        {
            Assert.Equal("Successfully added child", root.GetProperty("response").GetString());
        });
    }

    private void AssertSerializesBaseEvent<T>(
        T payload,
        string expectedEventName,
        Action<JsonElement>? extraAssertions = null) where T : GovEventPayload
    {
        var json = JsonSerializer.Serialize(payload, _jsonOptions);
        using var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;

        Assert.Equal("gov_event", root.GetProperty("event").GetString());
        Assert.Equal(expectedEventName, root.GetProperty("event_name").GetString());
        Assert.Equal(payload.ToolName, root.GetProperty("tool_name").GetString());
        Assert.Equal(payload.Section, root.GetProperty("section").GetString());
        Assert.Equal(payload.Action, root.GetProperty("action").GetString());
        Assert.Equal(payload.UiText, root.GetProperty("ui_text").GetString());
        Assert.Equal(payload.Type, root.GetProperty("type").GetString());

        extraAssertions?.Invoke(root);
    }
}
