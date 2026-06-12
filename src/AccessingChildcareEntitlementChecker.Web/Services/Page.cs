namespace AccessingChildcareEntitlementChecker.Web.Services;

public record Page(PageKey PageKey, IReadOnlyList<Edge> Edges)
{
    public PageKey? Next(EdgeContext context)
    {
        return Edges.FirstOrDefault(edge => edge.Condition(context))?.Target;
    }
}
