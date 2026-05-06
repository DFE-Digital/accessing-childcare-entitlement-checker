namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
