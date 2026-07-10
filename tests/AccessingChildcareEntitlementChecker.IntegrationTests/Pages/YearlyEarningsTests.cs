using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class YearlyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/earnings/adjusted-net-income";

    [Theory]
    [InlineData(null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(2)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, YearlyEarningsOption.AboveThreshold, null, null, "/benefits/benefits")]
    [InlineData(null, YearlyEarningsOption.BelowThreshold, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, null, null, "/benefits/benefits")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, BenefitsOption.CarersAllowance, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.BelowThreshold, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.BelowThreshold, null, UniversalCreditOption.Receives, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, YearlyEarningsOption yearlyEarnings, BenefitsOption? benefits, UniversalCreditOption? universalCredit, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            YearlyEarnings = yearlyEarnings,
            Benefits = benefits is null ? new() : [benefits.Value],
            UniversalCredit = universalCredit,
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
            new KeyValuePair<string, string>("YearlyEarnings", yearlyEarnings.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/earnings/wage")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        string backLinkUrl)
    {
        using var client = factory.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
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
}
