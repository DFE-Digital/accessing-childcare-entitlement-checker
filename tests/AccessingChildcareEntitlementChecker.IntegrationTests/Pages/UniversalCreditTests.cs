using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class UniversalCreditTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_PaidWork_No_Points_To_PaidWork()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.No };
        using var client = factory.CreateClientWithJourneyState(state);
        var res = await client.GetAsync("/User/UniversalCredit", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/User/PaidWork", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_SelfEmployedDuration_LessThan12_Points_To_SelfEmployedDuration()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.Yes, SelfEmployedDuration = SelfEmployedDurationOption.LessThan12Months };
        using var client = factory.CreateClientWithJourneyState(state);
        var res = await client.GetAsync("/User/UniversalCredit", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/User/SelfEmployedDuration", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_WeeklyEarnings_Above_YearlyEarnings()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.Yes, WeeklyEarnings = WeeklyEarningsOption.AboveThreshold };
        using var client = factory.CreateClientWithJourneyState(state);
        var res = await client.GetAsync("/User/UniversalCredit", TestContext.Current.CancellationToken);
        res.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(res.Content);
        var back = doc.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains("/User/YearlyEarnings", back.GetAttribute("href") ?? string.Empty);
    }

    [Fact]
    public async Task Post_Receives_Redirects_To_Benefits()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
        var get = await client.GetAsync("/User/UniversalCredit", TestContext.Current.CancellationToken);
        get.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(get.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(doc);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(get);

        var req = new HttpRequestMessage(HttpMethod.Post, "/User/UniversalCredit");
        if (cookie != null) req.Headers.Add("Cookie", cookie);
        req.Content = new FormUrlEncodedContent([
            new KeyValuePair<string,string>("__RequestVerificationToken", token ?? string.Empty),
            new KeyValuePair<string,string>("UniversalCredit", "Receives")
        ]);
        var post = await client.SendAsync(req, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.Redirect, post.StatusCode);
        Assert.Contains("/User/Benefits", post.Headers.Location?.ToString() ?? string.Empty);
    }
}
