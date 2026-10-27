using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Extensions;

public static class ChildCollectionExtensions
{
    public static Child? ResolveLastEditedChild(this IDictionary<string, Child> children, string? childId)
    {
        if (childId is not null &&
            children.TryGetValue(childId, out var child))
        {
            return child;
        }

        return children.Values.LastOrDefault();
    }
}
