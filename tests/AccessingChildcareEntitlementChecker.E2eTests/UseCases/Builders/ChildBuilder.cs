using System.Globalization;
using AccessingChildcareEntitlementChecker.E2eTests.Pages;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

internal sealed class ChildBuilder
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

    public ChildBuilder WithBirthDate(int addYears = 0, int addMonths = 0, int addDays = 0)
    {
        var date = DateTime.Today.AddYears(addYears).AddMonths(addMonths).AddDays(addDays);
        _steps.Add(new AnswerStep(PageNames.ChildBirthDate, date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
        return this;
    }

    public ChildBuilder WithDueDate(int addYears = 0, int addMonths = 0, int addDays = 0)
    {
        var date = DateTime.Today.AddYears(addYears).AddMonths(addMonths).AddDays(addDays);
        _steps.Add(new AnswerStep(PageNames.ChildDueDate, date.ToString("dd MMM yyyy", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
        return this;
    }

    public ChildBuilder WithSupport(string support)
    {
        _steps.Add(new AnswerStep(PageNames.ChildSupport, support));
        return this;
    }

    public IEnumerable<JourneyStep> Build() => _steps;
}
