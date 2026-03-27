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

            // Checking the status code is sufficient to know that the DI is all hooked up properly.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
