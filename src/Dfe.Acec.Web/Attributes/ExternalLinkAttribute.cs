namespace Dfe.Acec.Web.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public class ExternalLinkAttribute : Attribute
{
    public required string To { get; init; }
    public required string Description { get; init; }
    public required bool OpenInNewTab { get; init; }
}
