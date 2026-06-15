namespace AccessingChildcareEntitlementChecker.Web.Services;

public record ActionLink(string Controller, string Action, string? returnTo = null)
{
    public static ActionLink User(string action, string? returnTo = null) => new ActionLink("User", action, returnTo);

    public static readonly ActionLink CheckYourAnswers = new ActionLink("Summary", "CheckAnswers");
}
