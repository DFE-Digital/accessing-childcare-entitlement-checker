using Dfe.Acec.Web.Validators;
using FluentValidation;

namespace Dfe.Acec.Web.Services;

public class IncompleteChildHandler(JourneyState journeyState, IJourneySession journeySession, IValidator<JourneyState> journeyStateValidator)
{
    public List<string> CheckForIncompleteChildren()
    {
        var removedChildNames = new List<string>();

        var result = journeyStateValidator.Validate(
            journeyState,
            options => options.IncludeRuleSets(
                JourneyStateValidator.CheckChildDetailsRuleSet));

        if (!result.IsValid)
        {
            var invalidChildIds = result.Errors
                .Select(error => error.CustomState)
                .OfType<string>()
                .Distinct()
                .ToList();

            var invalidChildren = journeyState.Children
                .Where(x => invalidChildIds.Contains(x.Key))
                .ToList();

            removedChildNames = [.. invalidChildren.Select(x => x.Value.Name)];

            foreach (var child in invalidChildren)
            {
                journeyState.Children.Remove(child.Key);
            }

            journeySession.SetState(journeyState);
        }

        return removedChildNames;
    }
}
