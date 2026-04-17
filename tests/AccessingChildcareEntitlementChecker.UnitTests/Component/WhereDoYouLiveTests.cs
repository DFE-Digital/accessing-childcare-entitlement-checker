using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Net;

namespace AccessingChildcareEntitlementChecker.UnitTests.Component
{
    public class WhereDoYouLiveTests
    {
        private static WebApplicationFactory<Program> CreateFactory(string environmentName) =>
            new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment(environmentName);
                    builder.ConfigureAppConfiguration((_, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["DevelopmentBasicAuthPassword"] = "dev-only"
                        });
                    });
                });

        [Fact]
        public async Task GetRootReturnsWhereDoYouLivePage()
        {
            var factory = CreateFactory("Production");
            var client = factory.CreateClient();

            var response = await client.GetAsync("/");
            var body = await response.Content.ReadAsStringAsync();

            // Checking the status code is sufficient to know that the DI is all hooked up properly.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRootRequiresBasicAuthInDevelopment()
        {
            var factory = CreateFactory("Development");
            var client = factory.CreateClient();

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.Contains("Basic", response.Headers.WwwAuthenticate.ToString());
        }

        [Fact]
        public async Task GetRootSucceedsWithBasicAuthInDevelopment()
        {
            var factory = CreateFactory("Development");
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("user:dev-only")));

            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRobotsTxtReturnsNoIndexInstructionsInDevelopment()
        {
            var factory = CreateFactory("Development");
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
            var factory = CreateFactory("Production");
            var client = factory.CreateClient();

            var response = await client.GetAsync("/robots.txt");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetHealthCheckDoesNotRequireBasicAuthInDevelopment()
        {
            var factory = CreateFactory("Development");
            var client = factory.CreateClient();

            var response = await client.GetAsync("/health");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
