using System.Reflection;
using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases;

internal static class UseCaseRepository
{
    private static readonly Dictionary<string, IUseCase> UseCases;

    static UseCaseRepository()
    {
        UseCases = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IUseCase).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(t => (IUseCase)Activator.CreateInstance(t)!)
            .ToDictionary(u => u.Name, u => u, StringComparer.OrdinalIgnoreCase);
    }

    public static IEnumerable<JourneyStep> GetJourney(string useCaseName)
    {
        return UseCases.TryGetValue(useCaseName, out var useCase)
            ? useCase.GetJourney()
            : throw new KeyNotFoundException($"Use case '{useCaseName}' not found. Available use cases: {string.Join(", ", UseCases.Keys)}");
    }
}
