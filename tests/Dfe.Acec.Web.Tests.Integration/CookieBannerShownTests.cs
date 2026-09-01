using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration;

public class CookieBannerShownTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task GetWithoutCookieShowsBanner()
    {
        var sessionRequiredEndpoints = GetCookieBannerRequiredEndpoints();
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                       {
                           BirthStatus = BirthStatus.Born,
                           BirthDate = new DateOnly(2020, 1, 1),
                           ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
                       }
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            await using var getHost = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            using var getClient = getHost.CreateClient();

            var getResponse = await getClient.GetAsync(url, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            document.AssertCookieBanner();
        }
    }

    [Fact]
    public async Task GetWithCookieDoesNotShowBanner()
    {
        var sessionRequiredEndpoints = GetCookieBannerRequiredEndpoints();
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                       {
                           BirthStatus = BirthStatus.Born,
                           BirthDate = new DateOnly(2020, 1, 1),
                           ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
                       }
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            await using var getHost = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            using var getClient = getHost.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", "cookie_policy=enabled");
            var getResponse = await getClient.SendAsync(request, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            document.AssertNoCookieBanner();
        }
    }

    private IEnumerable<string> GetCookieBannerRequiredEndpoints()
    {
        var cookielessRoutes = new[]
        {
            "/throw",
            "/robots.txt",
        };

        return RouteHelper.GetEndpointsExcept(factory, "GET", cookielessRoutes);
    }
}
