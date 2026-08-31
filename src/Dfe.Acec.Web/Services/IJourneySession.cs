namespace Dfe.Acec.Web.Services;

public interface IJourneySession
{
    public bool HasSession { get; }
    public JourneyState GetState();
    public void SetState(JourneyState journeyState);
}
