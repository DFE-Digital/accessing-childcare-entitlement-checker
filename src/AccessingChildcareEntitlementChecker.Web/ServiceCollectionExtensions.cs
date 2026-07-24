using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.Web.Configuration;
using AccessingChildcareEntitlementChecker.Web.Services;
using Azure.Identity;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using AccessingChildcareEntitlementChecker.Web.Filters;

namespace AccessingChildcareEntitlementChecker.Web;

[ExcludeFromCodeCoverage]
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
            return journeySession.Get();
        });
        services.AddScoped<RequireJourneySessionFilter>();

        services.AddScoped<ICookiePolicyService, CookiePolicyService>();
        return services;
    }

    public static IServiceCollection AddGoogleTagManager(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<GoogleTagManagerOptions>()
            .Bind(configuration.GetSection(
                GoogleTagManagerOptions.SectionName))
            .Validate(
                options =>
                    !options.Enabled ||
                    !string.IsNullOrWhiteSpace(options.ContainerId),
                "A Google Tag Manager container ID is required when Google Tag Manager is enabled.")
            .Validate(
                options =>
                    !options.Enabled ||
                    options.ContainerId.StartsWith(
                        "GTM-",
                        StringComparison.Ordinal),
                "The Google Tag Manager container ID must start with 'GTM-'.")
            .ValidateOnStart();

        return services;
    }
}
