using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ChildDueDateTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string _childId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";
    private const string _url = $"/children/{_childId}/expectant-childs-due-date";

    [Theory]
    [InlineData(null, $"/children/{_childId}/has-the-child-been-born")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        _childId,
                        new Child(_childId, "Sara")
                    }
                }
        });

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertDateInput()
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, $"/children/check-childs-details")]
    [InlineData(ReturnTo.CheckAnswers, $"/children/check-childs-details")]
    [InlineData(ReturnTo.CheckChildDetails, $"/children/check-childs-details")]
    public async Task PostValidRedirects(string? returnTo, string continueUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        _childId,
                        new Child(_childId, "Sara")
                    }
                }
        });

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string, string>("ChildDueDate.Day", yesterday.Day.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildDueDate.Month", yesterday.Month.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildDueDate.Year", yesterday.Year.ToString(System.Globalization.CultureInfo.InvariantCulture))
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, $"/children/{_childId}/has-the-child-been-born")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostWithYesterdaysDateFailsValidationWithBackLink(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        _childId,
                        new Child(_childId, "Sara")
                    }
                }
        });

        using var client = host.CreateClient();

        var url = $"{_url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string, string>("ChildDueDate.Day", yesterday.Day.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildDueDate.Month", yesterday.Month.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildDueDate.Year", yesterday.Year.ToString(System.Globalization.CultureInfo.InvariantCulture))
            ],
            TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertHeading("What is this child's due date?")
                    .AssertValidationError()
                    .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task ReturnsNotFoundForNonexistantChild()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();
        var url = _url;
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
