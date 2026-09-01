namespace Dfe.Acec.Web.Tests.E2e.Pages;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class PagePatternAttribute(string pattern) : Attribute
{
    public string Pattern { get; } = pattern;
}
