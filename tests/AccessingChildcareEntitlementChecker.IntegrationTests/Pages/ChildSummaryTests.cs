using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildSummaryTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";
    const string OtherChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1f";

    /// <summary>
    /// When the user has arrived at the summary and no child is specified
    /// clicking back should return them to the last child in the ordered dict
    /// with the appropriate page.
    /// </summary>
    /// <returns>Task representing the result.</returns>
    [Theory]
    [InlineData(BirthStatus.Due, $"/children/{OtherChildId}/relationship-to-expectant-child")]
    [InlineData(BirthStatus.Born, $"/children/{OtherChildId}/child-benefits")]
    public async Task Get_BackLink_Is_To_Last_Child(BirthStatus birthStatus, string expectedUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                        {
                            BirthStatus = BirthStatus.Born,
                        }
                    },
                    {
                        OtherChildId,
                        new Child(OtherChildId, "Aydin")
                        {
                            BirthStatus = birthStatus,
                        }
                    }
            }
        });

        var url = $"/children/check-childs-details";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(expectedUrl);
    }

    /// <summary>
    /// When the user has arrived at the summary from a specific child
    /// clicking back should return them to that child.,
    /// </summary>
    /// <returns>Task representing the result.</returns>
    [Theory]
    [InlineData(OtherChildId, $"/children/{OtherChildId}/relationship-to-expectant-child")]
    [InlineData(ChildId, $"/children/{ChildId}/child-benefits")]
    public async Task Get_BackLink_Is_To_Specified_Child(string arrivedFromChildId, string expectedUrl)
    {
        
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                        {
                            BirthStatus = BirthStatus.Born,
                        }
                    },
                    {
                        OtherChildId,
                        new Child(OtherChildId, "Aydin")
                        {
                            BirthStatus = BirthStatus.Due,
                        }
                    }
            }
        });

        var url = $"/children/check-childs-details?childId={arrivedFromChildId}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(expectedUrl);
    }

    /// <summary>
    /// When the user has arrived at the summary then removed all the children,
    /// clicking back should take them to the add child details page.
    /// </summary>
    /// <returns>Task representing the result.</returns>
    [Fact]
    public async Task Get_BackLink_Is_To_Name()
    {
        using var client = factory.CreateClient();
        var url = $"/children/check-childs-details?childId={ChildId}"; // Should not matter what is passed here.
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink($"/children/add-child-details");
    }
}
