using System.Net;
using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class HasPartnerTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_HasPartner_Has_Radios_And_BackLink_Defaults_To_ChildcareSupport_Back()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var res = await client.GetAsync("/partner", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var radios = doc.QuerySelectorAll("input[type=radio][name=HasPartner]");
        Assert.Equal(2, radios.Length);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/benefits/childcare-support", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Get_HasPartner_BackLink_When_Vouchers_Points_To_ChildcareVoucherReceipt()
    {
        var state = new JourneyState();
        state.ChildcareSupport.Add(ChildcareSupportOption.ChildcareVouchers);
        using var client = factory.CreateClientWithJourneyState(state);
        var res = await client.GetAsync("/partner", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/benefits/childcare-vouchers", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Post_No_Redirects_To_CheckAnswers()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var get = await client.GetAsync("/partner", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var req = new HttpRequestMessage(HttpMethod.Post, "/partner");
        if (cookie != null) req.Headers.Add("Cookie", cookie);
        req.Content = new FormUrlEncodedContent([
            new KeyValuePair<string,string>("__RequestVerificationToken", token ?? string.Empty),
            new KeyValuePair<string,string>("HasPartner", "false")
        ]);
        var post = await client.SendAsync(req, TestContext.Current.CancellationToken);
        Assert.Equal(HttpStatusCode.Redirect, post.StatusCode);
        Assert.Contains("/check-your-answers", post.Headers.Location?.ToString() ?? string.Empty);
    }
}
