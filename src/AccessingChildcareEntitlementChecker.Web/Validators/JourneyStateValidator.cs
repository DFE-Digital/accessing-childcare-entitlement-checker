using AccessingChildcareEntitlementChecker.Web.Models;
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
             RuleForEach(x => x.Children.Values)
                .ChildRules(child =>
                {
                    child.RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithState(x => x.ChildId);

                    child.RuleFor(x => x.BirthStatus)
                        .NotNull()
                        .WithState(x => x.ChildId);
                    
                    child.When(x => x.BirthStatus == BirthStatus.Born, () =>
                    {
                        child.RuleFor(x => x.BirthDate)
                            .NotNull()
                            .WithState(x => x.ChildId);

                        child.RuleFor(x => x.ChildSupportOptions)
                            .NotEmpty()
                            .WithState(x => x.ChildId);
                    });

                    child.When(x => x.BirthStatus == BirthStatus.Due, () =>
                    {
                        child.RuleFor(x => x.DueDate)
                            .NotNull()
                            .WithState(x => x.ChildId);
                    });
                });
        });

        RuleSet(CheckAnswersRuleSet, () =>
        {
            // Boilerplate for final answers rules
        });
    }
}
