using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Run tests in Development environment for predictable behaviour
        builder.UseEnvironment("Development");
        return base.CreateHost(builder);
    }

    private class TestJourneySession(JourneyState state) : IJourneySession
    {
        private JourneyState _state = state;
        public JourneyState Get() => _state;
        public void Set(JourneyState journeyState) => _state = journeyState;
    }

    public HttpClient CreateClientWithJourneyState(JourneyState state)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // replace IJourneySession with test double and ensure JourneyState resolves to the same instance
                services.AddScoped<IJourneySession>(_ => new TestJourneySession(state));
                services.AddScoped(_ => state);
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }
}
