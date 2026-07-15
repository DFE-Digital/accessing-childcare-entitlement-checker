using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class ResponseAsserts
{
    public static HttpResponseMessage AssertRedirect(this HttpResponseMessage response, string expectedLink)
    {
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains(expectedLink, response.Headers.Location?.ToString() ?? string.Empty);
        return response;
    }

    public static HttpResponseMessage AssertNoContent(this HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        return response;
    }

    public static HttpResponseMessage AssertCookie(this HttpResponseMessage response, string key, string value)
    {
        var cookie = $"{key}={value};";
        var cookieHeaders = response.Headers.GetValues("Set-Cookie");
        Assert.Contains(cookieHeaders, c => c.StartsWith(cookie, StringComparison.OrdinalIgnoreCase));
        return response;
    }

    public static HttpResponseMessage AssertBadRequest(this HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        return response;
    }
}
