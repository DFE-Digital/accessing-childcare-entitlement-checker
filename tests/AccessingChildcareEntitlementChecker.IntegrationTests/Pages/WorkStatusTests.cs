using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class WorkStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_WorkStatus_Page_Has_Checkboxes_And_BackLink()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await client.GetAsync("/User/WorkStatus", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        // assert checkboxes count equals enum members (structure, not content)
        var checkboxes = doc.QuerySelectorAll("input[type=checkbox][name=WorkStatus]");
        Assert.Equal(3, checkboxes.Length);

        // assert back link is present and points to PaidWork action
        var backLink = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(backLink);
        Assert.Contains("/User/PaidWork", backLink.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Post_Invalid_Submission_Shows_Validation_Error()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // GET to obtain antiforgery token and cookie
        var get = await client.GetAsync("/User/WorkStatus", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var request = new HttpRequestMessage(HttpMethod.Post, "/User/WorkStatus");
        if (cookie != null) request.Headers.Add("Cookie", cookie);
        var form = new List<KeyValuePair<string, string>>
        {
            // no WorkStatus values submitted to trigger validation
            new("__RequestVerificationToken", token ?? string.Empty),
        };
        request.Content = new FormUrlEncodedContent(form);

        var post = await client.SendAsync(request, TestContext.Current.CancellationToken);
        post.EnsureSuccessStatusCode();

        var postDoc = await HtmlHelpers.ParseHtmlAsync(post.Content);

        // assert an inline error message (structure) is present
        var error = postDoc.QuerySelector(".govuk-error-message");
        Assert.NotNull(error);
    }

    [Fact]
    public async Task Post_Selection_Navigates_Forward()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        // obtain antiforgery token and cookie
        var get = await client.GetAsync("/User/WorkStatus", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var request = new HttpRequestMessage(HttpMethod.Post, "/User/WorkStatus");
        if (cookie != null) request.Headers.Add("Cookie", cookie);

        // choose SelfEmployed so flow redirects to SelfEmployedDuration
        var form = new List<KeyValuePair<string, string>>
        {
            new("__RequestVerificationToken", token ?? string.Empty),
            new("WorkStatus", "SelfEmployed"),
        };

        request.Content = new FormUrlEncodedContent(form);
        var post = await client.SendAsync(request, TestContext.Current.CancellationToken);

        // expect redirect (302) to SelfEmployedDuration
        Assert.Equal(System.Net.HttpStatusCode.Redirect, post.StatusCode);
        var location = post.Headers.Location?.ToString();
        Assert.NotNull(location);
        Assert.Contains("/User/SelfEmployedDuration", location);
    }
}
