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
        ExternalLinkAttribute? link = null;

        var result = HintService.ProcessHint(description, link);

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
    public void ProcessHintWhenDescriptionIsNullAndExternalLinkIsNotNullReturnsNull()
    {
        string? description = null;
        var link = new ExternalLinkAttribute
        {
            To = "MySite",
            OpenInNewTab = false,
        };

        var result = HintService.ProcessHint(description, link);

        Assert.Null(result);
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNotNullAndExternalLinkIsNotNullReturnsHtmlHint()
    {
        var description = "My link description";
        var link = new ExternalLinkAttribute
        {
            To = "MySite",
            OpenInNewTab = false
        };

        var result = HintService.ProcessHint(description, link);

        var expectedTag = new TemplateString("<a href=\"MySite\" rel=\"noreferrer\">My link description</a>");
        Assert.NotNull(result?.Html);
        Assert.Equal(expectedTag, result.Html);
    }

    [Fact]
    public void ProcessHintWhenDescriptionIsNotNullAndExternalLinkIsNotNullAndOpenInNewTabIsTrueReturnsHtmlHint()
    {
        var description = "My link description";
        var link = new ExternalLinkAttribute
        {
            To = "MySite",
            OpenInNewTab = true
        };

        var result = HintService.ProcessHint(description, link);

        var expectedTag = new TemplateString("<a href=\"MySite\" rel=\"noreferrer\" target=\"_blank\">My link description</a>");
        Assert.NotNull(result?.Html);
        Assert.Equal(expectedTag, result.Html);
    }
}
