namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public interface ITodayFactory
    {
        DateOnly Today { get; }
    }
}
