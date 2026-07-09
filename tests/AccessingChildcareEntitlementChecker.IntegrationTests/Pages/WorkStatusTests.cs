using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class WorkStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/work-status/work-status?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(3)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, WorkStatusOption workStatus, SelfEmployedDurationOption? selfEmployedDuration, WeeklyEarningsOption? weeklyEarnings, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            WorkStatus = [workStatus],
            SelfEmployedDuration = selfEmployedDuration,
            WeeklyEarnings = weeklyEarnings,
        });

        var url = $"/work-status/work-status?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("WorkStatus", workStatus.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/work-status/work")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"/work-status/work-status?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task Post_Selection_Redirects_To_SelfEmployed()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var url = "/work-status/work-status";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new("WorkStatus", "SelfEmployed"),
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/work-status/self-employed");
    }
}
