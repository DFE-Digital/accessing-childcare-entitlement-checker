namespace Dfe.Acec.Tests.E2e.Pages;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal sealed class PagePatternAttribute(string pattern) : Attribute
{
    public string Pattern { get; } = pattern;
}
