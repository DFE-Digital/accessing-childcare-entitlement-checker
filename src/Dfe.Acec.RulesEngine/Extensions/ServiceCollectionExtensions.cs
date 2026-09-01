using Dfe.Acec.RulesEngine.Evaluators;
using Dfe.Acec.RulesEngine.Schemes;
using Dfe.Acec.RulesEngine.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.Acec.RulesEngine.Extensions;

public static class ServiceCollectionExtensions
{
    [PublicAPI]
    public static IServiceCollection AddRulesEngine(
        this IServiceCollection services)
    {
        services.AddScoped<EntitlementRulesEngine>();

        services.AddScoped<ISchemeEvaluator, UniversalCreditChildcareEvaluator>();
        services.AddScoped<ISchemeEvaluator, FifteenHoursUniversalEvaluator>();
        services.AddScoped<ISchemeEvaluator, TaxFreeChildcareEvaluator>();
        services.AddScoped<ISchemeEvaluator, ThirtyHoursForWorkingFamiliesEvaluator>();
        services.AddScoped<ISchemeEvaluator, FifteenHoursForDisadvantagedChildrenEvaluator>();

        return services;
    }
}
