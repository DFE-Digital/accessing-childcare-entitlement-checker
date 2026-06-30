using System.Reflection;
using Microsoft.Playwright;

namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal class PageFactory(IPage page)
{
    private static readonly List<(string Pattern, Type PageType)> PageRegistry;

    static PageFactory()
    {
        PageRegistry = [];

        var pageObjectTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IPageObject).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var type in pageObjectTypes)
        {
            var attribute = type.GetCustomAttribute<PagePatternAttribute>()
                            ?? throw new InvalidOperationException($"Page Object type '{type.Name}' must be decorated with a [PagePattern] attribute.");

            PageRegistry.Add((attribute.Pattern, type));
        }
    }

    public IPageObject GetPage(string pageNameOrHeading)
    {
        var cleaned = pageNameOrHeading.Trim();

        foreach (var (pattern, pageType) in PageRegistry)
        {
            if (MatchesPattern(pattern, cleaned))
            {
                return (IPageObject)Activator.CreateInstance(pageType, page)!;
            }
        }

        throw new KeyNotFoundException($"No Page Object mapped for page: '{pageNameOrHeading}'");
    }

    private static bool MatchesPattern(string pattern, string input)
    {
        return pattern.Contains("__PLACEHOLDER__")
            ? PageNames.GetRegexForPattern(pattern).IsMatch(input)
            : string.Equals(pattern, input, StringComparison.OrdinalIgnoreCase);
    }
}
