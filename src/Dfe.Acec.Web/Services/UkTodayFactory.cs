namespace Dfe.Acec.Web.Services;

public class UkTodayFactory : ITodayFactory
{
    private readonly IDateTimeFactory _dateTimeFactory;
    private readonly TimeZoneInfo _ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");

    public UkTodayFactory(IDateTimeFactory dateTimeFactory)
    {
        _dateTimeFactory = dateTimeFactory;
    }

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
