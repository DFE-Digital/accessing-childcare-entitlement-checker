using System.Reflection;
using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Web.Tests.E2e.UseCases;

internal static class UseCaseRepository
{
    private static readonly Dictionary<string, IUseCase> _useCases;

    static UseCaseRepository()
    {
        _useCases = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IUseCase).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })
            .Select(t => (IUseCase)Activator.CreateInstance(t)!)
            .ToDictionary(u => u.Name, u => u, StringComparer.OrdinalIgnoreCase);
    }

    public static IEnumerable<JourneyStep> GetJourney(string useCaseName)
    {
        return _useCases.TryGetValue(useCaseName, out var useCase)
            ? useCase.GetJourney()
            : throw new KeyNotFoundException($"Use case '{useCaseName}' not found. Available use cases: {string.Join(", ", _useCases.Keys)}");
    }
}
