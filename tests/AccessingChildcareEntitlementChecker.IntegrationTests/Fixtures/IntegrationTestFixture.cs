using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        return base.CreateHost(builder);
    }

    public HttpClient CreateClientWithFeatureFlags(Dictionary<string, string?> featureFlags)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(featureFlags);
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    public HttpClient CreateClientWithJourneyState(JourneyState state)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IJourneySession>(_ => new TestJourneySession(state));
                services.AddScoped(_ => state);
                services.AddScoped<IValidator<JourneyState>, JourneyStateValidator>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    public HttpClient CreateClientWithJourneyStateAndFeatureFlags(JourneyState state, Dictionary<string, string?> featureFlags)
    {
        return WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(featureFlags);
            });
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
                services.AddScoped<IValidator<JourneyState>, JourneyStateValidator>();
            });
        }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    private sealed class TestJourneySession(JourneyState state) : IJourneySession
    {
        public bool HasSession => true;
        private JourneyState _state = state;
        public JourneyState GetState() => _state;
        public void SetState(JourneyState journeyState) => _state = journeyState;
    }

    private sealed class MissingJourneySession : IJourneySession
    {
        public bool HasSession => false;
        public JourneyState GetState() => new();
        public void SetState(JourneyState journeyState) { }
    }
}
