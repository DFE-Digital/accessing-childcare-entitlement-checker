using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace AccessingChildcareEntitlementChecker.UnitTests.Component
{
    public class WhereDoYouLiveTests
    {
        [Fact]
        public async Task GetRootReturnsWhereDoYouLivePage()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
            var client = factory.CreateClient();

            var response = await client.GetAsync("/");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Check what help you could get with childcare costs", body);
        }
    }
}
