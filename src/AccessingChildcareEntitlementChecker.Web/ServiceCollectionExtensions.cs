using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.Web.Services;
using Azure.Identity;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;

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

        return services;
    }
}
