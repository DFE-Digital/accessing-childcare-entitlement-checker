namespace Dfe.Acec.Web.Services;

public class UkTodayFactory(IDateTimeFactory dateTimeFactory) : ITodayFactory
{
    private readonly IDateTimeFactory _dateTimeFactory = dateTimeFactory;
    private readonly TimeZoneInfo _ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

    public DateOnly Today
    {
        get
        {
            var now = _dateTimeFactory.UtcNow;
            var ukNow = TimeZoneInfo.ConvertTime(now, _ukTimeZone);
            return DateOnly.FromDateTime(ukNow);
        }
    }
}
