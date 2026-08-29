using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

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
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            YearlyEarnings = yearlyEarnings,
        });

        using var client = host.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(9)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner()
            .AssertGroupHint("Select all that apply");
    }

    [Theory]
    [InlineData(null, BenefitsOption.CarersAllowance, null)]
    [InlineData(ReturnTo.CheckAnswers, BenefitsOption.CarersAllowance, null)]
    [InlineData(ReturnTo.CheckAnswers, BenefitsOption.CarersAllowance, ChildcareSupportOption.ChildcareVouchers)]
    public async Task PostValidRedirects(string? returnTo, BenefitsOption benefits, ChildcareSupportOption? childcareSupport)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Benefits = [benefits],
            ChildcareSupport = childcareSupport is null ? new() : [childcareSupport.Value],
        });

        using var client = host.CreateClient();
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

        postResponse.AssertRedirect("/benefits/childcare-support");
    }

    [Theory]
    [InlineData(null, YearlyEarningsOption.AboveThreshold, "/earnings/adjusted-net-income")]
    [InlineData(null, YearlyEarningsOption.BelowThreshold, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, YearlyEarningsOption.AboveThreshold, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(
        string? returnTo,
        YearlyEarningsOption? yearlyEarnings,
        string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            YearlyEarnings = yearlyEarnings,
        });

        using var client = host.CreateClient();

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
}
