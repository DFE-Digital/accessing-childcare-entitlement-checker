using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class NavigationService
{
    public JourneyState _journeyState;

    public NavigationService(JourneyState journeyState)
    {
        _journeyState = journeyState;
    }

    public bool UserAgeValid()
    {
        return _journeyState.Children.Count() > 0;
    }

    public bool NationalityValid()
    {
        return _journeyState.UserAge != null;
    }

    public bool SettledStatusValid()
    {
        return _journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland;
    }

    public bool PaidWorkValid()
    {
        if (_journeyState.Nationality == null)
        {
            return false;
        }

        if (_journeyState.Nationality ==
            NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland)
        {
            return _journeyState.SettledStatus != null;
        }

        return true;
    }

    public ActionLink UserAgePrevious(string? returnTo)
    {
        if (returnTo != null)
        {
            return ActionLink.CheckYourAnswers;
        }

        return ActionLink.User(nameof(SummaryController.CheckChildDetails));
    }

    public ActionLink NationalityPrevious(string? returnTo)
    {
        if (returnTo != null)
        {
            return ActionLink.CheckYourAnswers;
        }

        return ActionLink.User(nameof(UserController.UserAge));
    }

    public ActionLink SettledStatusPrevious(string? returnTo)
    {
        if (returnTo == null)
        {
            return ActionLink.User(nameof(UserController.Nationality));
        }

        return ActionLink.CheckYourAnswers;
    }

    public ActionLink PaidWorkPrevious(string? returnTo)
    {
        if (returnTo != null)
        {
            return ActionLink.CheckYourAnswers;
        }

        var previousAction = _journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            ? nameof(UserController.SettledStatus)
            : nameof(UserController.Nationality);
        return ActionLink.User(previousAction);
    }

    public RedirectToActionResult UserAgeNext(string? returnTo)
    {
        if (returnTo == null)
        {
            return new RedirectToActionResult(nameof(UserController.Nationality), "User", null);
        }

        return new RedirectToActionResult(nameof(SummaryController.CheckAnswers), "Summary", new { returnTo });
    }

    public RedirectToActionResult NationalityNext(string? returnTo)
    {
        var requiresSettledStatus = _journeyState.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland;
        var nextAction = requiresSettledStatus ? nameof(UserController.SettledStatus) : nameof(UserController.PaidWork);
        var nextAnswerMissing = requiresSettledStatus ? _journeyState.SettledStatus == null : _journeyState.PaidWork == null;

        if (returnTo is not null && !nextAnswerMissing)
        {
            return new RedirectToActionResult(nameof(SummaryController.CheckAnswers), "Summary", new { returnTo });
        }

        return new RedirectToActionResult(nextAction, "User", new { returnTo });
    }

    public RedirectToActionResult SettledStatusNext(string? returnTo)
    {
        var nextAction = nameof(UserController.PaidWork);
        var nextAnswerMissing = _journeyState.PaidWork == null;

        if (returnTo is not null && !nextAnswerMissing)
        {
            return new RedirectToActionResult(nameof(SummaryController.CheckAnswers), "Summary", new { returnTo });
        }

        return new RedirectToActionResult(nextAction, "User", new { returnTo });
    }

    public RedirectToActionResult PaidWorkNext(string? returnTo)
    {
        var nextAction = _journeyState.PaidWork switch
        {
            PaidWorkOption.Yes => nameof(UserController.WorkStatus),
            PaidWorkOption.No => nameof(UserController.UniversalCredit),
            PaidWorkOption.OnLeave => nameof(UserController.TypeOfLeave),
            _ => throw new InvalidOperationException("Unexpected PaidWorkOption value")
        };

        var nextAnswerMissing = _journeyState.PaidWork switch
        {
            PaidWorkOption.Yes => _journeyState.WorkStatus.Count == 0,
            PaidWorkOption.No => _journeyState.UniversalCredit == null,
            PaidWorkOption.OnLeave => true,
            _ => throw new InvalidOperationException("Unexpected PaidWorkOption value")
        };

        if (returnTo is not null && !nextAnswerMissing)
        {
            return new RedirectToActionResult(nameof(SummaryController.CheckAnswers), "Summary", new { returnTo });
        }

        return new RedirectToActionResult(nextAction, "User", new { returnTo });
    }
}
