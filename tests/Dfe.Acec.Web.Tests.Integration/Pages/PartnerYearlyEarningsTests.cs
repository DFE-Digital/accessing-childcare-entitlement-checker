using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class PartnerYearlyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/earnings/adjusted-net-income-partner";

    [Theory]
    [InlineData(null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, YearlyEarningsOption.AboveThreshold, null)]
    [InlineData(null, YearlyEarningsOption.BelowThreshold, null)]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, null)]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.AboveThreshold, PartnerBenefitsOption.CarersAllowance)]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.BelowThreshold, null)]
    [InlineData(ReturnTo.CheckAnswers, YearlyEarningsOption.BelowThreshold, PartnerBenefitsOption.CarersAllowance)]
    public async Task PostValidRedirects(string? returnTo, YearlyEarningsOption partnerYearlyEarnings, PartnerBenefitsOption? partnerBenefits)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerYearlyEarnings = partnerYearlyEarnings,
            PartnerBenefits = partnerBenefits is null ? new() : [partnerBenefits.Value],
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
            new KeyValuePair<string, string>("PartnerYearlyEarnings", partnerYearlyEarnings.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/Partner/PartnerBenefits");
    }

    [Theory]
    [InlineData(null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostInvalidShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

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
