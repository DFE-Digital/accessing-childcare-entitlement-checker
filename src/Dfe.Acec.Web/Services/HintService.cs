using Dfe.Acec.Web.Attributes;
using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.Acec.Web.Services;

public static class HintService
{
    public static HintOptions? ProcessHint(string? textDescription, ExternalLinkAttribute? externalLinkAttribute)
    {
        if (textDescription is null)
        {
            return null;
        }

        if (externalLinkAttribute is null)
        {
            return ProcessTextHint(textDescription);
        }

        return ProcessLinkHint(textDescription, externalLinkAttribute);
    }

    private static HintOptions ProcessTextHint(string description)
    {
        return new HintOptions
        {
            Text = description,
        };
    }

    private static HintOptions ProcessLinkHint(string description, ExternalLinkAttribute externalLinkAttribute)
    {
        var builder = new TagBuilder("a");
        builder.MergeAttribute("href", externalLinkAttribute.To);
        builder.MergeAttribute("rel", "noreferrer");
        if (externalLinkAttribute.OpenInNewTab)
        {
            builder.MergeAttribute("target", "_blank");
        }

        builder.InnerHtml.Append(description);
        var htmlText = builder.ToHtmlString();

        return new HintOptions
        {
            Html = htmlText,
        };
    }
}
