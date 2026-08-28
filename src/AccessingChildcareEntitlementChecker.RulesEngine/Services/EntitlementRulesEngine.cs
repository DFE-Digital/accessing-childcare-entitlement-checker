using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Services;

public class EntitlementRulesEngine
{
    private readonly IEnumerable<ISchemeEvaluator> _schemeEvaluators;

    public EntitlementRulesEngine(IEnumerable<ISchemeEvaluator> schemeEvaluators)
    {
        _schemeEvaluators = schemeEvaluators;
    }

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
                AgeInYears = child.AgeInYears,
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