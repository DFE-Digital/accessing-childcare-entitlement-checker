namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class PagePatternAttribute(string pattern) : Attribute
{
    public string Pattern { get; } = pattern;
}
