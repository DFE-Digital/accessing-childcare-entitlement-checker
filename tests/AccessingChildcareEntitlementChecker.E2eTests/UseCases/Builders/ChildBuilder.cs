using AccessingChildcareEntitlementChecker.E2eTests.Pages;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

internal class ChildBuilder
{
    private readonly List<JourneyStep> _steps = [];

    public ChildBuilder WithName(string name)
    {
        _steps.Add(new AnswerStep(PageNames.ChildName, name));
        return this;
    }

    public ChildBuilder IsBorn(string isBorn)
    {
        _steps.Add(new AnswerStep(PageNames.ChildIsBorn, isBorn));
        return this;
    }

    public ChildBuilder WithBirthDate(string birthDate)
    {
        _steps.Add(new AnswerStep(PageNames.ChildBirthDate, birthDate));
        return this;
    }

    public ChildBuilder WithDueDate(string dueDate)
    {
        _steps.Add(new AnswerStep(PageNames.ChildDueDate, dueDate));
        return this;
    }

    public ChildBuilder WithRelationship(string relationship)
    {
        _steps.Add(new AnswerStep(PageNames.ChildRelationship, relationship));
        return this;
    }

    public ChildBuilder WithExpectingRelationship(string relationship)
    {
        _steps.Add(new AnswerStep(PageNames.ExpectedChildRelationship, relationship));
        return this;
    }

    public ChildBuilder WithSupport(string support)
    {
        _steps.Add(new AnswerStep(PageNames.ChildSupport, support));
        return this;
    }

    public IEnumerable<JourneyStep> Build() => _steps;
}
