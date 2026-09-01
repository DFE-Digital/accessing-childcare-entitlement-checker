using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ChildIsBornTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";
    private const string Url = $"/children/{ChildId}/has-the-child-been-born";
    private static readonly DateOnly _tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
    private static readonly DateOnly _yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));

    [Theory]
    [InlineData(null, $"/children/add-child-details/{ChildId}")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
        });

        using var client = host.CreateClient();

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
    [InlineData(null, false, false, BirthStatus.Born, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(null, false, false, BirthStatus.Due, $"/children/{ChildId}/expectant-childs-due-date")]
    [InlineData(ReturnTo.CheckAnswers, true, false, BirthStatus.Born, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckAnswers, false, true, BirthStatus.Born, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckAnswers, false, true, BirthStatus.Due, $"/children/{ChildId}/expectant-childs-due-date")]
    [InlineData(ReturnTo.CheckChildDetails, true, false, BirthStatus.Born, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckChildDetails, false, true, BirthStatus.Born, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckChildDetails, false, true, BirthStatus.Due, $"/children/{ChildId}/expectant-childs-due-date")]
    [InlineData(ReturnTo.CheckChildDetails, true, false, BirthStatus.Due, $"/children/{ChildId}/expectant-childs-due-date")]
    public async Task PostValidRedirects(
        string? returnTo,
        bool hasBirthDate,
        bool hasDueDate,
        BirthStatus birthStatus,
        string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                        {
                            BirthDate = hasBirthDate ? _yesterday : null,
                            DueDate = hasDueDate ? _tomorrow : null,
                            BirthStatus = birthStatus,
                        }
                    }
                }
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
                new KeyValuePair<string,string>("ChildIsBorn", birthStatus.ToString())
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, $"/children/add-child-details/{ChildId}")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostWithInvalidShowsValidationErrorAndBackLink(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
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

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
                    .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task ReturnsNotFoundForNonexistantChild()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();
        var url = Url;
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
