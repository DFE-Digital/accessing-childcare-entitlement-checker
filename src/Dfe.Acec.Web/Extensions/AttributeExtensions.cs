using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Dfe.Acec.Web.Extensions;

public static class AttributeExtensions
{
    public static IEnumerable<TagHelperAttribute> GetGovG4Attributes(this TagHelperOutput output) => output.Attributes.Where(x => x.Name?.StartsWith("govg4-", StringComparison.InvariantCulture) ?? false);
}
