using System.Net;
using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PaidWorkTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_PaidWork_Has_Radios_And_BackLink()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var res = await client.GetAsync("work-status/work", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var radios = doc.QuerySelectorAll("input[type=radio][name=PaidWork]");
        Assert.Equal(3, radios.Length);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/nationality", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Get_PaidWork_BackLink_When_Nationality_EU_Points_To_SettledStatus()
    {
        var state = new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland };
        using var client = factory.CreateClientWithJourneyState(state);
        var res = await client.GetAsync("work-status/work", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/nationality/settled-status", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Post_Invalid_Shows_Validation_Error()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var get = await client.GetAsync("/work-status/work", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var req = new HttpRequestMessage(HttpMethod.Post, "/work-status/work");
        if (cookie != null) req.Headers.Add("Cookie", cookie);
        req.Content = new FormUrlEncodedContent([new KeyValuePair<string,string>("__RequestVerificationToken", token ?? string.Empty)
        ]);
        var post = await client.SendAsync(req, TestContext.Current.CancellationToken);
        post.EnsureSuccessStatusCode();
        var postDoc = await HtmlHelpers.ParseHtmlAsync(post.Content);
        Assert.NotNull(postDoc.QuerySelector(".govuk-error-message"));
    }

    [Fact]
    public async Task Post_OnLeave_Redirects_To_TypeOfLeave()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var get = await client.GetAsync("/work-status/work", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var req = new HttpRequestMessage(HttpMethod.Post, "/work-status/work");
        if (cookie != null) req.Headers.Add("Cookie", cookie);
        req.Content = new FormUrlEncodedContent([
            new KeyValuePair<string,string>("__RequestVerificationToken", token ?? string.Empty),
            new KeyValuePair<string,string>("PaidWork", "OnLeave")
        ]);
        var post = await client.SendAsync(req, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Redirect, post.StatusCode);
        Assert.Contains("/leave/type-of-leave", post.Headers.Location?.ToString() ?? string.Empty);
    }
}
