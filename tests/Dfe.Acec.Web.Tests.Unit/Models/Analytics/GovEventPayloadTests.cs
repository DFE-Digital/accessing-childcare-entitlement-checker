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
    public void SelectContentEventSerializesToExpectedJsonStructure()
    {
        // Arrange
        var payload = new SelectContentEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "add child",
            UiText = "Add a Child",
            Type = "button"
        };

        // Act
        var json = JsonSerializer.Serialize<GovEventPayload>(payload, _jsonOptions);

        // Assert
        using var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
            //test the mappings are the json names
        Assert.Equal("gov_event", root.GetProperty("event").GetString());
        Assert.Equal("select_content", root.GetProperty("event_name").GetString());
        Assert.Equal("childcare_eligibility_checker", root.GetProperty("tool_name").GetString());
        Assert.Equal("children details", root.GetProperty("section").GetString());
        Assert.Equal("add child", root.GetProperty("action").GetString());
        Assert.Equal("Add a Child", root.GetProperty("ui_text").GetString());

            // optional metrics must not be present in JSON output
        Assert.False(root.TryGetProperty("metric_name", out _));
        Assert.False(root.TryGetProperty("metric_value", out _));
    }

    [Fact]
    public void FormErrorEventSerializesOutcomeProperty()
    {
        // Arrange
        var payload = new FormErrorEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Outcome = "Select if you have a partner",
            Action = "error",
            UiText = "Continue",
            Type = "contact"
        };

        // Act
        var json = JsonSerializer.Serialize(payload, _jsonOptions);

        // Assert
        using var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
            //test the mappings are the json names
        Assert.Equal("gov_event", root.GetProperty("event").GetString());
        Assert.Equal("form_error", root.GetProperty("event_name").GetString());
        Assert.Equal("Select if you have a partner", root.GetProperty("outcome").GetString());
    }

    [Fact]
    public void FormResponseEventSerializesResponseProperty()
    {
        // Arrange
        var payload = new FormResponseEvent
        {
            ToolName = "childcare_eligibility_checker",
            Section = "children details",
            Action = "submit",
            UiText = "Continue",
            Type = "contact",
            Response = "Successfully added child"
        };

        // Act
        var json = JsonSerializer.Serialize(payload, _jsonOptions);

        // Assert
        using var jsonDoc = JsonDocument.Parse(json);
        var root = jsonDoc.RootElement;
        //test the mappings are the json names
        Assert.Equal("gov_event", root.GetProperty("event").GetString());
        Assert.Equal("form_response", root.GetProperty("event_name").GetString());
        Assert.Equal("Successfully added child", root.GetProperty("response").GetString());
    }
}
