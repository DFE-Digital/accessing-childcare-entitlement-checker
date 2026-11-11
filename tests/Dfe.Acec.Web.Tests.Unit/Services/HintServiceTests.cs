using Dfe.Acec.Web.Attributes;
using Dfe.Acec.Web.Services;
using GovUk.Frontend.AspNetCore.ComponentGeneration;

namespace Dfe.Acec.Web.Tests.Unit.Services;

public class HintServiceTests
{
    [Fact]
    public void ProcessHintWhenDescriptionAndExternalLinkAreNullReturnsNull()
    {
        string? description = null;
        ExternalLinkAttribute? externalLinkAttribute = null;

        var result = HintService.ProcessHint(description, externalLinkAttribute);

        Assert.Null(result);
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNotNullAndExternalLinkIsNullReturnsTextHint()
    {
        var description = "Hello from my description!";
        ExternalLinkAttribute? externalLinkAttribute = null;

        var result = HintService.ProcessHint(description, externalLinkAttribute);

        Assert.NotNull(result?.Text);
        Assert.Null(result.Html);
        Assert.Equal(description, result.Text);
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNullAndExternalLinkIsNotNullReturnsHtmlHintWithoutTargetBlank()
    {
        string? description = null;
        var externalLinkAttribute = new ExternalLinkAttribute
        {
            To = "MyTestLink",
            Description = "MyTestDescription",
            OpenInNewTab = false,
        };

        var result = HintService.ProcessHint(description, externalLinkAttribute);

        Assert.NotNull(result?.Html);
        Assert.Null(result.Text);

        var expected = new TemplateString("<a href=\"MyTestLink\">MyTestDescription</a>");
        Assert.True(result.Html.Equals(expected));
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNullAndExternalLinkIsNotNullWithOpenInNewTabReturnsHtmlHintWithTargetBlank()
    {
        string? description = null;
        var externalLinkAttribute = new ExternalLinkAttribute
        {
            To = "MyTestLink",
            Description = "MyTestDescription",
            OpenInNewTab = true,
        };

        var result = HintService.ProcessHint(description, externalLinkAttribute);

        Assert.NotNull(result?.Html);
        Assert.Null(result.Text);

        var expected = new TemplateString("<a href=\"MyTestLink\" target=\"_blank\">MyTestDescription</a>");
        Assert.True(result.Html.Equals(expected));
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNotNullAndExternalLinkIsNotNullPrioritiesDescription()
    {
        var description = "MyTextDescription";
        var externalLinkAttribute = new ExternalLinkAttribute
        {
            To = "MyTestLink",
            Description = "MyLinkDescription",
            OpenInNewTab = false,
        };

        var result = HintService.ProcessHint(description, externalLinkAttribute);

        Assert.NotNull(result?.Text);
        Assert.Null(result.Html);

        Assert.Equal(description, result.Text);
    }
}
