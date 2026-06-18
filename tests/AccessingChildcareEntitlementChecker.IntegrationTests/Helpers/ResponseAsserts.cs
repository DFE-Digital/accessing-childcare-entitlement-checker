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
}
