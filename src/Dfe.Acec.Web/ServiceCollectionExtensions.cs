using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Dfe.Acec.Web.Mappers;
using Dfe.Acec.RulesEngine.Extensions;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Dfe.Acec.Web.Services.Summary;

namespace Dfe.Acec.Web;

[ExcludeFromCodeCoverage]
[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedCacheConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration["RedisConnection"];

        if (string.IsNullOrWhiteSpace(redisConnection))
        {
            services.AddDistributedMemoryCache();
            return services;
        }

        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var options = ConfigurationOptions.Parse(redisConnection);

            options.Protocol = RedisProtocol.Resp3;

            options
                .ConfigureForAzureWithTokenCredentialAsync(new DefaultAzureCredential())
                .GetAwaiter()
                .GetResult();

            return ConnectionMultiplexer.Connect(options);
        });

        services.AddOptions<RedisCacheOptions>()
            .Configure<IConnectionMultiplexer>((options, multiplexer) =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer);
            });

        services.AddStackExchangeRedisCache(_ => { });

        return services;
    }


    public static IServiceCollection AddJourneyServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeFactory, DateTimeFactory>();
        services.AddScoped<ITodayFactory, UkTodayFactory>();
        services.AddScoped<IJourneySession, JourneySession>();
        services.AddScoped(sp =>
        {
            var journeySession = sp.GetRequiredService<IJourneySession>();
            return journeySession.GetState();
        });
        services.AddScoped<RequireJourneySessionFilter>();

        services.AddScoped<ICookiePolicyService, CookiePolicyService>();
        services.AddScoped<IValidator<JourneyState>, JourneyStateValidator>();

        services.AddScoped<IUserSummaryBuilder, UserSummaryBuilder>();
        services.AddScoped<IPartnerSummaryBuilder, PartnerSummaryBuilder>();
        services.AddScoped<IChildSummaryBuilder, ChildSummaryBuilder>();
        services.AddScoped<ISummaryViewModelBuilder, SummaryViewModelBuilder>();

        return services;
    }

    public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration, CookieSecurePolicy securePolicy)
    {
        services
            .AddDistributedCacheConfiguration(configuration)
            .AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = securePolicy;
                options.Cookie.SameSite = SameSiteMode.Lax;
            })
            .AddAntiforgery(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = securePolicy;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        return services;
    }

    public static IServiceCollection AddWebComponents(this IServiceCollection services)
    {
        services
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .AddHttpContextAccessor()
            .AddGovUkFrontend(options =>
            {
                options.GetCspNonceForRequest = context => context.Items["csp-nonce"]?.ToString();
            })
            .AddHealthChecks();

        services.AddFeatureManagement();

        services
            .AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();

        services.AddJourneyServices();

        services.AddSingleton<EntitlementResponseToResultsSummaryViewModelMapper>();
        services.AddSingleton<EntitlementResponseToResultsDetailsViewModelMapper>();
        services.AddSingleton<JourneyStateToEntitlementRequestMapper>();
        services.AddRulesEngine();

        return services;
    }

    public static IServiceCollection AddTelemetryAndProxies(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            services.AddOpenTelemetry().UseAzureMonitor();
        }

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto;
            options.KnownProxies.Clear();
            options.KnownIPNetworks.Clear();
            options.AllowedHosts = ["*.azurefd.net", "check-if-you-are-eligible-for-childcare-funding.education.gov.uk"];
        });

        return services;
    }
}
