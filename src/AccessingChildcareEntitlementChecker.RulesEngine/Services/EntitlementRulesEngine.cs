using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Services;

public class EntitlementRulesEngine
{
    private readonly IEnumerable<ISchemeEvaluator> schemeEvaluators;

    public EntitlementRulesEngine(IEnumerable<ISchemeEvaluator> schemeEvaluators)
    {
        this.schemeEvaluators = schemeEvaluators;
    }

    public EntitlementResponse Evaluate(
        EntitlementRequest request,
        DateOnly today)
    {
        var context = DerivedContextBuilder.Build(request, today);

        var childResults = context.Children
            .Select(child => new ChildResultDto
            {
                ChildName = child.Name,

                Schemes = schemeEvaluators
                    .Select(evaluator => evaluator.Evaluate(context, child))
                    .Where(result => result is not null)
                    .Cast<SchemeResultDto>()
                    .ToList()
            })
            .ToList();

        return new EntitlementResponse
        {
            ChildResults = childResults
        };
    }
}