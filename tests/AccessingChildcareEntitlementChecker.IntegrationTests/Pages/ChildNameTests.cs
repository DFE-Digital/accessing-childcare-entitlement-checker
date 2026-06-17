using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildNameTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Post_With_Long_Name_Triggers_Validation()
    {
        var client = factory.CreateClient();
        var page = await client.GetPageAsync("/Introduction/ChildName", TestContext.Current.CancellationToken);
        var submitted = await client.SubmitPageAsync(
            page,
            [
                new KeyValuePair<string, string>("ChildName", new string('A', 61))
            ]);

        submitted.EnsureSuccessStatusCode();
        var validationFailedPage = await submitted.ReadContentAsPageAsync();
        validationFailedPage.AssertValidationError();
    }
}
