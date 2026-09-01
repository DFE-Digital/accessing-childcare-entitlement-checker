using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Models;

public static class ReturnTo
{
    public const string CheckChildDetails = "check-your-childrens-details";

    public const string CheckAnswers = "check-your-answers";

    public static bool TryGetReturnToUrl(IUrlHelper urlHelper, string? returnTo, [NotNullWhen(true)] out string? url)
    {
        return TryGetReturnToUrl(urlHelper, returnTo, null, out url);
    }

    public static bool TryGetReturnToUrl(IUrlHelper urlHelper, string? returnTo, string? childId, [NotNullWhen(true)] out string? url)
    {
        url = returnTo switch
        {
            CheckChildDetails => urlHelper.Action(nameof(Controllers.SummaryController.CheckChildDetails), Controllers.SummaryController.Name, new { childId }),
            CheckAnswers => urlHelper.Action(nameof(Controllers.SummaryController.CheckAnswers), Controllers.SummaryController.Name, new { childId }),
            _ => null,
        };

        return url != null;
    }
}
