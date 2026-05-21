using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;
using AccessingChildcareEntitlementChecker.RulesEngine.Schemes;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRulesEngine(
        this IServiceCollection services)
    {
        services.AddScoped<EntitlementRulesEngine>();

        services.AddScoped<ISchemeEvaluator, UniversalCreditChildcareEvaluator>();
        services.AddScoped<ISchemeEvaluator, FifteenHoursUniversalEvaluator>();

        return services;
    }
}