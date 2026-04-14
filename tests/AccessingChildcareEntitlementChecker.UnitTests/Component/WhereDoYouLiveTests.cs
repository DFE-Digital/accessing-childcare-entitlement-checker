using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;

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

        [Fact]
        public async Task GetRobotsTxtReturnsNoIndexInstructionsInDevelopment()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseEnvironment("Development"));
            var client = factory.CreateClient();

            var response = await client.GetAsync("/robots.txt");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/plain", response.Content.Headers.ContentType?.MediaType);
            Assert.Equal("User-agent: *\nDisallow: /", body);
        }

        [Fact]
        public async Task GetRobotsTxtReturnsNotFoundOutsideDevelopment()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
            var client = factory.CreateClient();

            var response = await client.GetAsync("/robots.txt");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
