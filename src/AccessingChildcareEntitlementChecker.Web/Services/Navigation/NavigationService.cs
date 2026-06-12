using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation;

public class NavigationService(
    IEnumerable<IWorkflowStep> steps,
    JourneyState journeyState,
    LinkGenerator linkGenerator,
    IHttpContextAccessor httpContextAccessor)
    : INavigationService
{
    private const string HomeController = "Home";
    private const string IntroductionController = "Introduction";
    private const string BornChildDetailsController = "BornChildDetails";
    private const string ExpectedChildDetailsController = "ExpectedChildDetails";
    private const string UserController = "User";
    private const string PartnerController = "Partner";
    private const string SummaryController = "Summary";
    private const string ResultsController = "Results";

    private static readonly Dictionary<Page, (string Controller, string Action)> PageRoutes = new()
    {
        { Page.Location, (HomeController, "Location") },
        { Page.ChildName, (IntroductionController, "ChildName") },
        { Page.IsChildBorn, (IntroductionController, "IsChildBorn") },
        { Page.ChildBirthDate, (BornChildDetailsController, "ChildBirthDate") },
        { Page.ChildRelationship, (BornChildDetailsController, "ChildRelationship") },
        { Page.ChildSupport, (BornChildDetailsController, "ChildSupport") },
        { Page.ChildDueDate, (ExpectedChildDetailsController, "ChildDueDate") },
        { Page.ExpectedChildRelationship, (ExpectedChildDetailsController, "ExpectedChildRelationship") },
        { Page.UserAge, (UserController, "UserAge") },
        { Page.Nationality, (UserController, "Nationality") },
        { Page.SettledStatus, (UserController, "SettledStatus") },
        { Page.PaidWork, (UserController, "PaidWork") },
        { Page.WorkStatus, (UserController, "WorkStatus") },
        { Page.TypeOfLeave, (UserController, "TypeOfLeave") },
        { Page.SelfEmployedDuration, (UserController, "SelfEmployedDuration") },
        { Page.WeeklyEarnings, (UserController, "WeeklyEarnings") },
        { Page.YearlyEarnings, (UserController, "YearlyEarnings") },
        { Page.UniversalCredit, (UserController, "UniversalCredit") },
        { Page.Benefits, (UserController, "Benefits") },
        { Page.ChildcareSupport, (UserController, "ChildcareSupport") },
        { Page.ChildcareVoucherReceipt, (UserController, "ChildcareVoucherReceipt") },
        { Page.HasPartner, (UserController, "HasPartner") },
        { Page.PartnerAge, (PartnerController, "PartnerAge") },
        { Page.PartnerNationality, (PartnerController, "PartnerNationality") },
        { Page.PartnerSettledStatus, (PartnerController, "PartnerSettledStatus") },
        { Page.PartnerPaidWork, (PartnerController, "PartnerPaidWork") },
        { Page.PartnerWorkStatus, (PartnerController, "PartnerWorkStatus") },
        { Page.PartnerTypeOfLeave, (PartnerController, "PartnerTypeOfLeave") },
        { Page.PartnerSelfEmployedDuration, (PartnerController, "PartnerSelfEmployedDuration") },
        { Page.PartnerWeeklyEarnings, (PartnerController, "PartnerWeeklyEarnings") },
        { Page.PartnerYearlyEarnings, (PartnerController, "PartnerYearlyEarnings") },
        { Page.PartnerBenefits, (PartnerController, "PartnerBenefits") },
        { Page.PartnerChildcareSupport, (PartnerController, "PartnerChildcareSupport") },
        { Page.PartnerChildcareVoucherReceipt, (PartnerController, "PartnerChildcareVoucherReceipt") },
        { Page.CheckAnswers, (SummaryController, "CheckAnswers") },
        { Page.CheckChildDetails, (SummaryController, "CheckChildDetails") },
        { Page.Results, (ResultsController, "Results") }
    };

    public string GetNextUrl(Page currentPage, string? returnTo = null, string? childId = null)
    {
        switch (returnTo)
        {
            case Models.ReturnTo.CheckAnswers:
                return GetUrl(Page.CheckAnswers, childId: childId);
            case Models.ReturnTo.CheckChildDetails:
                return GetUrl(Page.CheckChildDetails, childId: childId);
        }

        var machine = BuildMachine(currentPage, childId);
        machine.Fire(NavigationAction.Next);
        return GetUrl(machine.State, returnTo, childId);
    }

    public string GetBackUrl(Page currentPage, string? returnTo = null, string? childId = null)
    {
        switch (returnTo)
        {
            case Models.ReturnTo.CheckAnswers:
                return GetUrl(Page.CheckAnswers, childId: childId);
            case Models.ReturnTo.CheckChildDetails:
                return GetUrl(Page.CheckChildDetails, childId: childId);
        }

        var machine = BuildMachine(currentPage, childId);
        if (!machine.CanFire(NavigationAction.Back))
        {
            return "/";
        }

        machine.Fire(NavigationAction.Back);
        return GetUrl(machine.State, returnTo, childId);
    }

    private StateMachine<Page, NavigationAction> BuildMachine(Page startState, string? childId)
    {
        var machine = new StateMachine<Page, NavigationAction>(startState);

        foreach (var step in steps)
        {
            step.Configure(machine, journeyState, childId);
        }

        return machine;
    }

    protected virtual string GetUrl(Page page, string? returnTo = null, string? childId = null)
    {
        if (!PageRoutes.TryGetValue(page, out var route))
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page route mapping not found.");
        }

        var routeValues = new RouteValueDictionary();
        if (!string.IsNullOrEmpty(childId))
        {
            routeValues.Add(page is Page.CheckAnswers or Page.CheckChildDetails ? "fromChildId" : "childId", childId);
        }
        if (!string.IsNullOrEmpty(returnTo) && page is not Page.CheckAnswers && page is not Page.CheckChildDetails)
        {
            routeValues.Add("returnTo", returnTo);
        }

        var httpContext = httpContextAccessor.HttpContext;
        var url = linkGenerator.GetPathByAction(
            httpContext: httpContext!,
            action: route.Action,
            controller: route.Controller,
            values: routeValues);

        return url ?? "/";
    }
}
