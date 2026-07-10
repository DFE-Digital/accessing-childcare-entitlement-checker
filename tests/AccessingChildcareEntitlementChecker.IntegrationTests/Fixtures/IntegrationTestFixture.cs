using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        return base.CreateHost(builder);
    }

    public HttpClient CreateClientWithJourneyState(JourneyState state)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IJourneySession>(_ => new TestJourneySession(state));
                services.AddScoped(_ => state);
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    public HttpClient CreateClientWithoutJourneySession()
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IJourneySession, MissingJourneySession>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    private class TestJourneySession(JourneyState state) : IJourneySession
    {
        public bool HasSession => true;
        private JourneyState _state = state;
        public JourneyState Get() => _state;
        public void Set(JourneyState journeyState) => _state = journeyState;
    }

    private class MissingJourneySession : IJourneySession
    {
        public bool HasSession => false;
        public JourneyState Get() => new();
        public void Set(JourneyState journeyState) { }
    }
}
