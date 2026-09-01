using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Requests;
using Dfe.Acec.RulesEngine.Dtos.Responses;
using Dfe.Acec.RulesEngine.Evaluators;

namespace Dfe.Acec.RulesEngine.Services;

public class EntitlementRulesEngine(IEnumerable<ISchemeEvaluator> schemeEvaluators)
{
    private readonly IEnumerable<ISchemeEvaluator> _schemeEvaluators = schemeEvaluators;

    public EntitlementResponse Evaluate(
        EntitlementRequest request,
        DateOnly today)
    {
        var context = DerivedContextBuilder.Build(request, today);

        var childResults = new List<ChildResultDto>();

        foreach (var child in context.Children)
        {
            var schemes = new List<SchemeResultDto>();

            foreach (var evaluator in _schemeEvaluators)
            {
                var result = evaluator.Evaluate(context, child);

                if (result is not null)
                {
                    schemes.Add(result);
                }
            }

            childResults.Add(new ChildResultDto
            {
                ChildId = child.ChildId,
                ChildName = child.Name,
                IsBorn = child.IsBorn,
                Schemes = schemes
            });
        }

        return new EntitlementResponse
        {
            ChildResults = childResults,
            HasAccessToPublicFunds = context.Household.HasAccessToPublicFunds,
        };
    }
}
