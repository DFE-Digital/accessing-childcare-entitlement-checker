using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class HasPartnerTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Get_HasPartner_Has_Radios_And_BackLink_Defaults_To_ChildcareSupport_Back()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var doc = await HttpClientHelpers.GetDocumentAsync(client, "/partner", TestContext.Current.CancellationToken);
        doc.AssertRadioButtonCount(2)
            .AssertBackLink("/benefits/childcare-support");
    }

    [Fact]
    public async Task Get_HasPartner_BackLink_When_Vouchers_Points_To_ChildcareVoucherReceipt()
    {
        var state = new JourneyState();
        state.ChildcareSupport.Add(ChildcareSupportOption.ChildcareVouchers);
        using var client = factory.CreateClientWithJourneyState(state);

        var doc = await HttpClientHelpers.GetDocumentAsync(client, "/partner", TestContext.Current.CancellationToken);
        doc.AssertBackLink("/benefits/childcare-vouchers");
    }

    [Fact]
    public async Task Post_No_Redirects_To_CheckAnswers()
    {
        using var client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

        var response = await HttpClientHelpers.PostFormAsync(client, "/partner", [
            new KeyValuePair<string,string>("HasPartner", "false")
        ], TestContext.Current.CancellationToken);
        response.AssertRedirect("/check-your-answers");
    }
}
