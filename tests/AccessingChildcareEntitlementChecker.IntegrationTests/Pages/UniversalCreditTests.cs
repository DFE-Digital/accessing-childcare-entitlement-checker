using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class UniversalCreditTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_PaidWork_No_Points_To_PaidWork()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.No };
        using var client = factory.CreateClientWithJourneyState(state);

        var response = await client.GetAsync("/benefits/universal-credit", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink("/work-status/work");
    }

    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_SelfEmployedDuration_LessThan12_Points_To_SelfEmployedDuration()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.Yes, SelfEmployedDuration = SelfEmployedDurationOption.LessThan12Months };
        using var client = factory.CreateClientWithJourneyState(state);

        var response = await client.GetAsync("/benefits/universal-credit", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink("/work-status/self-employed");
    }

    [Fact]
    public async Task Get_UniversalCredit_BackLink_When_WeeklyEarnings_Above_YearlyEarnings()
    {
        var state = new JourneyState { PaidWork = PaidWorkOption.Yes, WeeklyEarnings = WeeklyEarningsOption.AboveThreshold };
        using var client = factory.CreateClientWithJourneyState(state);

        var response = await client.GetAsync("/benefits/universal-credit", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink("/earnings/adjusted-net-income");
    }

    [Fact]
    public async Task Post_Receives_Redirects_To_Benefits()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var url = "/benefits/universal-credit";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string,string>("UniversalCredit", "Receives")
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/benefits/benefits");
    }
}
