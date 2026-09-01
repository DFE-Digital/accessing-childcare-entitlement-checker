using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;

namespace Dfe.Acec.Web;

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

    [PublicAPI]
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
        return services;
    }

}
