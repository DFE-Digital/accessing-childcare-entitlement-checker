using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class HttpClientExtensions
{
    public static async Task<Page> GetPageAsync(this HttpClient client, string url, CancellationToken cancellationToken)
    {
        var get = await client.GetAsync(url, cancellationToken);
        get.EnsureSuccessStatusCode();
        return await get.ReadContentAsPageAsync();
    }

    public static Task<HttpResponseMessage> SubmitPageAsync(this HttpClient client, Page page, IEnumerable<KeyValuePair<string, string>> fields)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, page.Url);
        if (page.Cookie != null)
        {
            req.Headers.Add("Cookie", page.Cookie);
        }

        var nameValueCollection = fields.Append(new KeyValuePair<string, string>("__RequestVerificationToken", page.Token ?? string.Empty));
        req.Content = new FormUrlEncodedContent(nameValueCollection);
        return client.SendAsync(req, TestContext.Current.CancellationToken);
    }
}
