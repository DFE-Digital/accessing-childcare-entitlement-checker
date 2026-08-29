namespace Dfe.Acec.Web.Services;

public interface IJourneySession
{
    bool HasSession { get; }
    JourneyState GetState();
    void SetState(JourneyState journeyState);
}
