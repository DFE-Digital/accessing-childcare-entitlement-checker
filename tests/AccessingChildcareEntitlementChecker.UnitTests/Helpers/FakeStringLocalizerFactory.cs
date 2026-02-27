using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.UnitTests.Helpers;
public sealed class FakeStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource) => new FakeStringLocalizer();

    public IStringLocalizer Create(string baseName, string location) => new FakeStringLocalizer();
}

public sealed class FakeStringLocalizer : IStringLocalizer
{
    public LocalizedString this[string name] => new(name, name, resourceNotFound: true);

    public LocalizedString this[string name, params object[] arguments]
        => new(name, string.Format(name, arguments), resourceNotFound: true);

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        => Array.Empty<LocalizedString>();

    public IStringLocalizer WithCulture(System.Globalization.CultureInfo culture) => this;
}