using AccessingChildcareEntitlementChecker.RulesEngine.Derived;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;

namespace AccessingChildcareEntitlementChecker.RulesEngine.Evaluators;

public interface ISchemeEvaluator
{
    SchemeResultDto? Evaluate(
        DerivedContext context,
        ChildFacts child);
}