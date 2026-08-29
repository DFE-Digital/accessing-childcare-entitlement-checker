using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Responses;

namespace Dfe.Acec.RulesEngine.Evaluators;

public interface ISchemeEvaluator
{
    SchemeResultDto? Evaluate(
        DerivedContext context,
        ChildFacts child);
}