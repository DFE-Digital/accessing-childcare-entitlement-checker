using Dfe.Acec.Web.Attributes;
using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfe.Acec.Web.Services;

public static class HintService
{
    public static HintOptions? ProcessHint(string? textDescription, ExternalLinkAttribute? externalLinkAttribute)
    {
        return (textDescription, externalLinkAttribute) switch
        {
            (null, null) => null,
            // Taking the view that if both the Description & ExternalLink are configured, set the hint as the Description
            (not null, not null) => ProcessTextHint(textDescription),
            (not null, null) => ProcessTextHint(textDescription),
            (null, not null) => ProcessLinkHint(externalLinkAttribute)
        };
    }

    private static HintOptions? ProcessTextHint(string? description)
    {
        return description is null ? null : new HintOptions
        {
            Text = description,
        };
    }

    private static HintOptions? ProcessLinkHint(ExternalLinkAttribute externalLinkAttribute)
    {
        var builder = new TagBuilder("a");
        builder.MergeAttribute("href", externalLinkAttribute.To);
        if (externalLinkAttribute.OpenInNewTab)
        {
            builder.MergeAttribute("target", "_blank");
        }
        
        builder.InnerHtml.Append(externalLinkAttribute.Description);
        var htmlText = builder.ToHtmlString();

        return new HintOptions
        {
            Html = htmlText,
        };
    }
}