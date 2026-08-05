using AccessingChildcareEntitlementChecker.Web.Services;
using FluentValidation;

namespace AccessingChildcareEntitlementChecker.Web.Validators;

public class JourneyStateValidator : AbstractValidator<JourneyState>
{
    public const string CheckChildDetailsRuleSet = "CheckChildDetails";
    public const string CheckAnswersRuleSet = "CheckAnswers";

    public JourneyStateValidator()
    {
        RuleSet(CheckChildDetailsRuleSet, () =>
        {
            // Boilerplate for child details rules
        });

        RuleSet(CheckAnswersRuleSet, () =>
        {
            // Boilerplate for final answers rules
        });
    }
}
