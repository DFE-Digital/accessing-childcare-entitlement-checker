using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public record Journey(PageKey StartPage, IReadOnlyDictionary<PageKey, Page> Pages)
    {
        public IActionResult Forwards(Controller current, JourneyState journeyState, object? routeValues = null)
        {
            var context = EdgeContext.From(current, journeyState, routeValues);
            if (TryResolveReturnToPage(context, out var returnToPage))
            {
                return new RedirectToActionResult(
                    returnToPage.ActionName,
                    returnToPage.ControllerName,
                    context.RouteValuesWithoutReturnTo());
            }

            var next = Pages[PageKey.From(current)].Next(context);

            return next is null
                ? new NotFoundResult()
                : new RedirectToActionResult(next.ActionName, next.ControllerName, context.RouteValues);
        }

        public BackLinkHref Backwards(Controller current, JourneyState journeyState, object? routeValues = null)
        {
            var context = EdgeContext.From(current, journeyState, routeValues);
            var previous = TryResolveReturnToPage(context, out var returnToPage)
                ? returnToPage
                : ResolvePreviousPage(PageKey.From(current), context);

            var href = current.Url.Action(
                previous.ActionName,
                previous.ControllerName,
                context.RouteValuesWithoutReturnTo());

            if (href is null)
            {
                throw new InvalidOperationException(
                    $"Could not generate back link for {previous.ControllerName}.{previous.ActionName}.");
            }

            return new BackLinkHref(href);
        }

        private PageKey ResolvePreviousPage(PageKey current, EdgeContext context)
        {
            var candidates = GetPreviousPages(current, context)
                .Where(candidate => CanReachStart(candidate, context, []))
                .ToList();

            return candidates.Count switch
            {
                1 => candidates.Single(),
                0 => throw new InvalidOperationException(
                    $"No backwards transition found for {current.ControllerName}.{current.ActionName}."),
                _ => throw new InvalidOperationException(
                    $"Multiple backwards transitions found for {current.ControllerName}.{current.ActionName}: {Format(candidates)}.")
            };
        }

        private IReadOnlyList<PageKey> GetPreviousPages(PageKey target, EdgeContext context)
        {
            return Pages.Values
                .Where(page => page.Next(context) == target)
                .Select(page => page.PageKey)
                .Distinct()
                .ToList();
        }

        private bool CanReachStart(PageKey current, EdgeContext context, HashSet<PageKey> visited)
        {
            if (current == StartPage)
            {
                return true;
            }

            if (!visited.Add(current))
            {
                return false;
            }

            return GetPreviousPages(current, context)
                .Any(predecessor => CanReachStart(predecessor, context, visited));
        }

        private static bool TryResolveReturnToPage(EdgeContext context, out PageKey pageKey)
        {
            pageKey = context.RouteValues["returnTo"]?.ToString() switch
            {
                ReturnTo.CheckAnswers => new PageKey("Summary", "CheckAnswers"),
                ReturnTo.CheckChildDetails => new PageKey("Summary", "CheckChildDetails"),
                _ => null!
            };

            return pageKey is not null;
        }

        private static string Format(IEnumerable<PageKey> pages)
        {
            return string.Join(", ", pages.Select(page => $"{page.ControllerName}.{page.ActionName}"));
        }
    }
}
