namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public class UkTodayFactory : ITodayFactory
    {
        private readonly IDateTimeFactory _dateTimeFactory;
        private readonly TimeZoneInfo UkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

        public UkTodayFactory(IDateTimeFactory dateTimeFactory)
        {
            _dateTimeFactory = dateTimeFactory;
        }

        public DateOnly Today
        {
            get
            {
                var now = _dateTimeFactory.UtcNow;
                var ukNow = TimeZoneInfo.ConvertTime(now, UkTimeZone);
                return DateOnly.FromDateTime(ukNow);
            }
        }
    }
}
