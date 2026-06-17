using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildSupportTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Post_With_None_And_Another_Option_Selected_Fails_Validation_And_Preserves_Childs_Name()
    {
        var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        "9fbb8965-c988-4199-8b40-189efcfe2a1e",
                        new Child("9fbb8965-c988-4199-8b40-189efcfe2a1e", "Sara")
                        {
                            BirthDate = new DateOnly(2020, 1, 1)
                        }
                    }
                }
        });

        var page = await client.GetPageAsync("/BornChildDetails/ChildSupport?childId=9fbb8965-c988-4199-8b40-189efcfe2a1e", TestContext.Current.CancellationToken);
        var submitted = await client.SubmitPageAsync(
            page,
            [
                new KeyValuePair<string, string>("ChildSupportOptions", "ArmedForcesIndependencePayment"),
                new KeyValuePair<string, string>("ChildSupportOptions", "NoneOfTheseApply"),
            ]);

        submitted.EnsureSuccessStatusCode();
        var validationFailedPage = await submitted.ReadContentAsPageAsync();
        Assert.Equal("Does Sara get any of the following support?", validationFailedPage.H1);
    }
}
