using AccessingChildcareEntitlementChecker.E2eTests.Pages;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

internal class JourneyBuilder
{
    private readonly List<JourneyStep> _journey = [];

    public JourneyBuilder StartInLocation(string location)
    {
        _journey.Add(new AnswerStep(PageNames.Location, location));
        return this;
    }

    public JourneyBuilder AddChild(Action<ChildBuilder> buildChild)
    {
        var childBuilder = new ChildBuilder();
        buildChild(childBuilder);
        _journey.AddRange(childBuilder.Build());
        return this;
    }

    public JourneyBuilder SetUserAge(string age)
    {
        _journey.Add(new AnswerStep(PageNames.UserAge, age));
        return this;
    }

    public JourneyBuilder SetNationality(string nationality)
    {
        _journey.Add(new AnswerStep(PageNames.Nationality, nationality));
        return this;
    }

    public JourneyBuilder SetPaidWork(string paidWork)
    {
        _journey.Add(new AnswerStep(PageNames.PaidWork, paidWork));
        return this;
    }

    public JourneyBuilder SetWorkStatus(string status)
    {
        _journey.Add(new AnswerStep(PageNames.WorkStatus, status));
        return this;
    }

    public JourneyBuilder SetWeeklyEarnings(string weeklyEarnings)
    {
        _journey.Add(new AnswerStep(PageNames.WeeklyEarnings, weeklyEarnings));
        return this;
    }

    public JourneyBuilder SetYearlyEarnings(string yearlyEarnings)
    {
        _journey.Add(new AnswerStep(PageNames.YearlyEarnings, yearlyEarnings));
        return this;
    }

    public JourneyBuilder SetUniversalCredit(string universalCredit)
    {
        _journey.Add(new AnswerStep(PageNames.UniversalCredit, universalCredit));
        return this;
    }

    public JourneyBuilder SetBenefits(string benefits)
    {
        _journey.Add(new AnswerStep(PageNames.Benefits, benefits));
        return this;
    }

    public JourneyBuilder SetChildcareSupport(string childcareSupport)
    {
        _journey.Add(new AnswerStep(PageNames.ChildcareSupport, childcareSupport));
        return this;
    }

    public JourneyBuilder SetHasPartner(string hasPartner)
    {
        _journey.Add(new AnswerStep(PageNames.HasPartner, hasPartner));
        return this;
    }

    public JourneyBuilder Action(string actionName)
    {
        _journey.Add(new ActionStep(actionName));
        return this;
    }

    public IEnumerable<JourneyStep> Build() => _journey;
}
