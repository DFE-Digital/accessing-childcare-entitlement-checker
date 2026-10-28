using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Internal;

namespace Dfe.Acec.Web.Tests.Integration.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public class IntegrationTestFixture : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        // File versions are used for cachbusters on resources. (asp-append-version)
        // Don't do this during tests - it blows a standard 256 inotify handler budget on linux.
        builder.ConfigureServices(services =>
            services.AddSingleton<IFileVersionProvider, UnversionedFileVersionProvider>());
        return base.CreateHost(builder);
    }

    public WebApplicationFactory<Program> CreateClientWithFeatureFlags(Dictionary<string, string?> featureFlags)
    {
        var webHost = WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(featureFlags);
            });
        });
        webHost.ClientOptions.AllowAutoRedirect = false;
        return webHost;
    }

    public WebApplicationFactory<Program> CreateClientWithJourneyState(JourneyState state)
    {
        var webHost = WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IJourneySession>(_ => new TestJourneySession(state));
                services.AddScoped(_ => state);
                services.AddScoped<IValidator<JourneyState>, JourneyStateValidator>();
            });
        });
        webHost.ClientOptions.AllowAutoRedirect = false;
        return webHost;
    }

    public WebApplicationFactory<Program> CreateClientWithJourneyStateAndFeatureFlags(JourneyState state, Dictionary<string, string?> featureFlags)
    {
        var webHost = WithWebHostBuilder(builder =>
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
        });
        webHost.ClientOptions.AllowAutoRedirect = false;
        return webHost;
    }

    public WebApplicationFactory<Program> CreateClientWithoutJourneySession()
    {
        var webHost = WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IJourneySession, MissingJourneySession>();
                services.AddScoped<IValidator<JourneyState>, JourneyStateValidator>();
            });
        });
        webHost.ClientOptions.AllowAutoRedirect = false;
        return webHost;
    }

    public WebApplicationFactory<Program> CreateClientWithSessionClock(ISystemClock clock)
    {
        var webHost = WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.Configure<MemoryDistributedCacheOptions>(options => options.Clock = clock);
            });
        });
        webHost.ClientOptions.AllowAutoRedirect = false;
        return webHost;
    }

    private sealed class UnversionedFileVersionProvider : IFileVersionProvider
    {
        public string AddFileVersionToPath(PathString requestPathBase, string path) => path;
    }

    private sealed class TestJourneySession(JourneyState state) : IJourneySession
    {
        public bool HasSession => true;
        private JourneyState _state = state;
        public JourneyState GetState() => _state;
        public void SetState(JourneyState journeyState) => _state = journeyState;
        public void Clear() { }
    }

    private sealed class MissingJourneySession : IJourneySession
    {
        public bool HasSession => false;
        public JourneyState GetState() => new();
        public void SetState(JourneyState journeyState) { }
        public void Clear() { }
    }
}
