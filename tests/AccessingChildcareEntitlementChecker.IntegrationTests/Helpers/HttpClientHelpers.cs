using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public class HttpClientHelpers
{
    public static async Task<HttpResponseMessage> PostFormAsync(
        HttpClient client,
        string url,
        string cookie,
        string token,
        IEnumerable<KeyValuePair<string, string>> formFields,
        CancellationToken cancellationToken)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, url);
        if (cookie != null)
        {
            req.Headers.Add("Cookie", cookie);
        }

        var nameValueCollection = formFields.Append(new KeyValuePair<string, string>("__RequestVerificationToken", token ?? string.Empty));
        req.Content = new FormUrlEncodedContent(nameValueCollection);
        return await client.SendAsync(req, cancellationToken);
    }
}
