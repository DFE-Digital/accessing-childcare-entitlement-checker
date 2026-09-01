using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class PartnerWorkStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/work-status/work-status-partner";

    [Theory]
    [InlineData(null, "/work-status/work-partner")]
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
            .AssertBetaBanner()
            .AssertCheckboxCount(3)
            .AssertGroupHint("Select all that apply");
    }

    [Theory]
    [InlineData(null, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed-partner")]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, null, null, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.LessThan12Months, null, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, null, "/earnings/wage-partner")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, WeeklyEarningsOption.AboveThreshold, "/earnings/wage-partner")]
    public async Task PostValidRedirects(string? returnTo, WorkStatusOption partnerWorkStatus, SelfEmployedDurationOption? partnerSelfEmployedDuration, WeeklyEarningsOption? partnerWeeklyEarnings, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerWorkStatus = [partnerWorkStatus],
            PartnerSelfEmployedDuration = partnerSelfEmployedDuration,
            PartnerWeeklyEarnings = partnerWeeklyEarnings,
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
            new KeyValuePair<string, string>("PartnerWorkStatus", partnerWorkStatus.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, "/work-status/work-partner")]
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
