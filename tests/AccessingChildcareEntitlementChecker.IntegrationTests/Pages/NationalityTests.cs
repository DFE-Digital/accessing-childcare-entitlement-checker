using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class NationalityTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_Nationality_Has_Radios_And_BackLink()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var res = await client.GetAsync("/User/Nationality", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var radios = doc.QuerySelectorAll("input[type=radio][name=Nationality]");
        Assert.Equal(3, radios.Length);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/User/UserAge", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Post_EU_Redirects_To_SettledStatus()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var get = await client.GetAsync("/User/Nationality", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var req = new HttpRequestMessage(HttpMethod.Post, "/User/Nationality");
        if (cookie != null) req.Headers.Add("Cookie", cookie);
        req.Content = new FormUrlEncodedContent([
            new KeyValuePair<string,string>("__RequestVerificationToken", token ?? string.Empty),
            new KeyValuePair<string,string>("Nationality", "CitizenOfAnEUCountryEEACountryOrSwitzerland")
        ]);
        var post = await client.SendAsync(req, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.Redirect, post.StatusCode);
        Assert.Contains("/User/SettledStatus", post.Headers.Location?.ToString() ?? string.Empty);
    }
}
