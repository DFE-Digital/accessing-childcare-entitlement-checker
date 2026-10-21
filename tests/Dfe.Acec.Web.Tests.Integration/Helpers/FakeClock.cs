using Microsoft.Extensions.Internal;

namespace Dfe.Acec.Web.Tests.Integration.Helpers;

public class FakeClock : ISystemClock
{
    public DateTimeOffset UtcNow { get; private set; } = DateTimeOffset.UtcNow;

    public void Advance(TimeSpan duration) => UtcNow += duration;
}
