namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public record SummaryRowViewModel(
        string Key,
        string Param,
        string Value,
        string ChangeController,
        string ChangeAction,
        string ChildId);
}
