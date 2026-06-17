using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class HttpResponseMessageExtensions
{
    public static async Task<Page> ReadContentAsPageAsync(this HttpResponseMessage response)
    {
        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(document);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(response);
        var uri = response.RequestMessage?.RequestUri?.ToString() ?? throw new InvalidOperationException("No request URI");
        return new Page(uri, document, token, cookie);
    }
}
