using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class PartnerParentalLeaveTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/leave/parental-leave-partner";
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";

    [Theory]
    [InlineData(null, "/work-status/work-partner")]
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
                },
        });

        using var client = host.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertCheckboxCount(2)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner()
            .AssertGroupHint("Select all that apply");
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, WorkStatusOption.SelfEmployed)]
    [InlineData(ReturnTo.CheckAnswers, null)]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.SelfEmployed)]
    public async Task PostValidRedirects(
        string? returnTo,
        WorkStatusOption? workStatus)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                },
            PartnerWorkStatus = workStatus == null ? [] : [workStatus.Value],
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
            new KeyValuePair<string, string>("PartnerParentalLeaveChildrenIds", ChildId),
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect("/work-status/work-status-partner");
    }

    [Theory]
    [InlineData(null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
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

    [Theory]
    [InlineData(null, "/work-status/work-partner")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    public async Task PostInvalidNoneWithSelectionShowsValidationError(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                },
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
            new KeyValuePair<string, string>("PartnerParentalLeaveChildrenIds", ChildId),
            new KeyValuePair<string, string>("PartnerParentalLeaveChildrenIds", "None")
        ], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task GetChildCheckboxLabelMaskedForClarity()
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

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        var childLabel = document.QuerySelector(".govuk-checkboxes__item");

        Assert.NotNull(childLabel);
        Assert.Equal("true", childLabel.GetAttribute("data-clarity-mask"));
        Assert.Contains("Sara", childLabel.TextContent);
    }
}
