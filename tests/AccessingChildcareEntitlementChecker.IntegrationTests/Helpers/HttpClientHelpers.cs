using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public class HttpClientHelpers
{
    public static async Task<HttpResponseMessage> PostFormAsync(HttpClient client, string url, IEnumerable<KeyValuePair<string, string>> formFields, CancellationToken cancellationToken)
    {
        var response = await client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(document);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(response);
        var uri = response.RequestMessage?.RequestUri?.ToString() ?? throw new InvalidOperationException("No request URI");
        var req = new HttpRequestMessage(HttpMethod.Post, uri);
        if (cookie != null)
        {
            req.Headers.Add("Cookie", cookie);
        }

        var nameValueCollection = formFields.Append(new KeyValuePair<string, string>("__RequestVerificationToken", token ?? string.Empty));
        req.Content = new FormUrlEncodedContent(nameValueCollection);
        return await client.SendAsync(req, cancellationToken);
    }
}
