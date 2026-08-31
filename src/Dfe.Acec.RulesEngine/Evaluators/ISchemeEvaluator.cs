using Dfe.Acec.RulesEngine.Derived;
using Dfe.Acec.RulesEngine.Dtos.Responses;

namespace Dfe.Acec.RulesEngine.Evaluators;

public interface ISchemeEvaluator
{
    public SchemeResultDto? Evaluate(
        DerivedContext context,
        ChildFacts child);
}
