using System.Net;
using JetBrains.Annotations;

namespace Dfe.Acec.Web.Tests.Integration.Helpers;

public static class ResponseAsserts
{
    //noinspection ParameterOnlyUsedForPreconditionCheck.Global
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

    [PublicAPI]
    public static HttpResponseMessage AssertCookie(this HttpResponseMessage response, string key, string value)
    {
        var cookie = $"{key}={value};";
        var cookieHeaders = response.Headers.GetValues("Set-Cookie");
        Assert.Contains(cookieHeaders, c => c.StartsWith(cookie, StringComparison.OrdinalIgnoreCase));
        return response;
    }

    [PublicAPI]
    public static HttpResponseMessage AssertBadRequest(this HttpResponseMessage response)
    {
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        return response;
    }
}
