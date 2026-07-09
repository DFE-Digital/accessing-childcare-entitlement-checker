using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class BenefitsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/benefits/benefits";

    [Theory]
    [InlineData(null, YearlyEarningsOption.AboveThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, YearlyEarningsOption.BelowThreshold, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, YearlyEarningsOption.AboveThreshold, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            YearlyEarnings = yearlyEarnings,
        });

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(9)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, BenefitsOption.CarersAllowance, null, "/benefits/childcare-support")]
    [InlineData(ReturnTo.CheckAnswers, BenefitsOption.CarersAllowance, null, "/benefits/childcare-support")]
    [InlineData(ReturnTo.CheckAnswers, BenefitsOption.CarersAllowance, ChildcareSupportOption.ChildcareVouchers, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, BenefitsOption benefits, ChildcareSupportOption? childcareSupport, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Benefits = [benefits],
            ChildcareSupport = childcareSupport is null ? new() : [childcareSupport.Value],
        });
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("Benefits", benefits.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, YearlyEarningsOption.AboveThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, YearlyEarningsOption.BelowThreshold, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, YearlyEarningsOption.AboveThreshold, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            YearlyEarnings = yearlyEarnings,
        });

        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(
            client,
            url,
            cookie,
            token,
            [],
            TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task GetWithoutSessionRedirectsToExpiry()
    {
        using var client = factory.CreateClientWithoutJourneySession();
        var getResponse = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        getResponse.AssertRedirect("/session-expired");
    }

    [Fact]
    public async Task PostWithoutSessionRedirectsToExpiry()
    {
        using var client = factory.CreateClientWithoutJourneySession();
        var postResponse = await client.PostAsync(Url, null, TestContext.Current.CancellationToken);
        postResponse.AssertRedirect("/session-expired");
    }
}
