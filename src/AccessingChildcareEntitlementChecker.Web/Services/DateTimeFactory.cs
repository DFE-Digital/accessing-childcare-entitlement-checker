using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Services
{
    [ExcludeFromCodeCoverage(Justification = "This class is not testable")]
    public class DateTimeFactory : IDateTimeFactory
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
