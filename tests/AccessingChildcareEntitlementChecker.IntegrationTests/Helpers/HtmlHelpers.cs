using AngleSharp.Html.Parser;
using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class HtmlHelpers
{
    public static async Task<IDocument> ParseHtmlAsync(HttpContent content)
    {
        var html = await content.ReadAsStringAsync();
        var parser = new HtmlParser();
        return await parser.ParseDocumentAsync(html);
    }

    public static string? ExtractAntiforgeryCookie(HttpResponseMessage response)
    {
        if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
        {
            var cookie = cookies.FirstOrDefault(c => c.Contains("Antiforgery") || c.Contains(".AspNetCore.Antiforgery"));
            if (cookie != null)
            {
                var pair = cookie.Split(';', 2)[0];
                return pair;
            }
        }

        return null;
    }

    public static string? ExtractAntiforgeryToken(IDocument document)
    {
        var input = document.QuerySelector("input[name=__RequestVerificationToken]") as AngleSharp.Html.Dom.IHtmlInputElement;
        return input?.GetAttribute("value");
    }
}
