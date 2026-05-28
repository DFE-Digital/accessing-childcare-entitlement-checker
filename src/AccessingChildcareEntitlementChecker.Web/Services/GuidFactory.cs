namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public class GuidFactory : IGuidFactory
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
