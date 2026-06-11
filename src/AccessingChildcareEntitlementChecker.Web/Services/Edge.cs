namespace AccessingChildcareEntitlementChecker.Web.Services;

public record Edge(Func<EdgeContext, bool> Condition, PageKey Target);
