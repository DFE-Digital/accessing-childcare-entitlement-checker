using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration.Pages;

public class ChildSummaryTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/children/check-childs-details";
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";
    private const string OtherChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1f";

    /// <summary>
    /// When the user has arrived at the summary and no child is specified
    /// clicking back should return them to the last child in the ordered dict
    /// with the appropriate page.
    /// </summary>
    [Theory]
    [InlineData(BirthStatus.Due, $"/children/{OtherChildId}/expectant-childs-due-date")]
    [InlineData(BirthStatus.Born, $"/children/{OtherChildId}/child-benefits")]
    public async Task Get(BirthStatus birthStatus, string expectedUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
            {
                {
                    ChildId,
                    CreateBornChild(ChildId, "Sara")
                },
                {
                    OtherChildId,
                    birthStatus == BirthStatus.Born
                        ? CreateBornChild(OtherChildId, "Aydin")
                        : CreateDueChild(OtherChildId, "Aydin")
                }
            }
        });

        using var client = host.CreateClient();

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        doc.AssertBackLink(expectedUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    /// <summary>
    /// When the user has arrived at the summary from a specific child
    /// clicking back should return them to that child.
    /// </summary>
    [Theory]
    [InlineData(OtherChildId, $"/children/{OtherChildId}/expectant-childs-due-date")]
    [InlineData(ChildId, $"/children/{ChildId}/child-benefits")]
    public async Task GetBackLinkIsToSpecifiedChild(
        string arrivedFromChildId,
        string expectedUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
            {
                {
                    ChildId,
                    CreateBornChild(ChildId, "Sara")
                },
                {
                    OtherChildId,
                    CreateDueChild(OtherChildId, "Aydin")
                }
            }
        });

        using var client = host.CreateClient();

        var url = $"{Url}?childId={arrivedFromChildId}";

        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        doc.AssertBackLink(expectedUrl);
    }

    /// <summary>
    /// When the user has arrived at the summary then removed all the children,
    /// clicking back should take them to the add child details page.
    /// </summary>
    [Fact]
    public async Task GetBackLinkIsToName()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var url = $"{Url}?childId={ChildId}";

        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        doc.AssertBackLink("/children/add-child-details");
    }

    private static Child CreateBornChild(string childId, string name)
    {
        return new Child(childId, name)
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };
    }

    private static Child CreateDueChild(string childId, string name)
    {
        return new Child(childId, name)
        {
            BirthStatus = BirthStatus.Due,
            DueDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(3))
        };
    }
}
