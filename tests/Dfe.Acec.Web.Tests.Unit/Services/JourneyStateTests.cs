using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Tests.Unit.Services;

public class JourneyStateTests
{
    private readonly JourneyState _journeyState = new();

    [Fact]
    public void GetChildReturnsNullIfChildDoesNotExist()
    {
        Assert.False(_journeyState.Children.TryGetValue("non-existent-child-id", out _));
    }

    [Fact]
    public void ApplyChildNameThrowsIfNoChildName()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            _journeyState.Apply(new ChildNameViewModel());
        });
    }

    [Fact]
    public void ApplyChildNameSetsChildIdIfNull()
    {
        var model = new ChildNameViewModel { ChildName = "Child A" };
        _journeyState.Apply(model);

        Assert.NotNull(model.ChildId);
    }

    [Fact]
    public void ApplyChildNameAddsChildIdIfNotExisting()
    {
        var model = new ChildNameViewModel { ChildName = "Child A" };
        _journeyState.Apply(model);

        Assert.Single(_journeyState.Children.Keys);
    }

    [Theory]
    [InlineData(PaidWorkOption.Yes)]
    [InlineData(PaidWorkOption.ParentalLeave)]
    [InlineData(PaidWorkOption.SickLeave)]
    [InlineData(PaidWorkOption.No)]
    public void ApplyPaidWorkKeepsLaterAnswersIfOptionIsUnchanged(PaidWorkOption option)
    {
        _journeyState.PaidWork = option;
        _journeyState.WorkStatus = [WorkStatusOption.SelfEmployed];
        _journeyState.ParentalLeaveChildrenIds = ["child-a"];
        _journeyState.SelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;
        _journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        _journeyState.YearlyEarnings = YearlyEarningsOption.BelowThreshold;

        _journeyState.Apply(new PaidWorkViewModel { PaidWork = option });

        Assert.Equal(option, _journeyState.PaidWork);
        Assert.Equal([WorkStatusOption.SelfEmployed], _journeyState.WorkStatus);
        Assert.Equal(["child-a"], _journeyState.ParentalLeaveChildrenIds);
        Assert.Equal(SelfEmployedDurationOption.NotLessThan12Months, _journeyState.SelfEmployedDuration);
        Assert.Equal(WeeklyEarningsOption.AboveThreshold, _journeyState.WeeklyEarnings);
        Assert.Equal(YearlyEarningsOption.BelowThreshold, _journeyState.YearlyEarnings);
    }

    [Fact]
    public void ApplyPaidWorkClearsParentalLeaveChildrenIfChangedToYes()
    {
        _journeyState.PaidWork = PaidWorkOption.ParentalLeave;
        _journeyState.ParentalLeaveChildrenIds = ["child-a"];

        _journeyState.Apply(new PaidWorkViewModel { PaidWork = PaidWorkOption.Yes });

        Assert.Empty(_journeyState.ParentalLeaveChildrenIds);
    }

    [Fact]
    public void ApplyPaidWorkClearsWeeklyEarningsIfChangedToParentalLeave()
    {
        _journeyState.PaidWork = PaidWorkOption.Yes;
        _journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        _journeyState.Apply(new PaidWorkViewModel { PaidWork = PaidWorkOption.ParentalLeave });

        Assert.Null(_journeyState.WeeklyEarnings);
    }

    [Fact]
    public void ApplyPaidWorkClearsLeaveAndEarningsIfChangedToSickLeave()
    {
        _journeyState.PaidWork = PaidWorkOption.ParentalLeave;
        _journeyState.ParentalLeaveChildrenIds = ["child-a"];
        _journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        _journeyState.Apply(new PaidWorkViewModel { PaidWork = PaidWorkOption.SickLeave });

        Assert.Empty(_journeyState.ParentalLeaveChildrenIds);
        Assert.Null(_journeyState.WeeklyEarnings);
    }

    [Fact]
    public void ApplyPaidWorkClearsWorkAnswersIfChangedToNo()
    {
        _journeyState.PaidWork = PaidWorkOption.Yes;
        _journeyState.WorkStatus = [WorkStatusOption.SelfEmployed];
        _journeyState.ParentalLeaveChildrenIds = ["child-a"];
        _journeyState.SelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;
        _journeyState.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        _journeyState.YearlyEarnings = YearlyEarningsOption.BelowThreshold;

        _journeyState.Apply(new PaidWorkViewModel { PaidWork = PaidWorkOption.No });

        Assert.Empty(_journeyState.WorkStatus);
        Assert.Empty(_journeyState.ParentalLeaveChildrenIds);
        Assert.Null(_journeyState.SelfEmployedDuration);
        Assert.Null(_journeyState.WeeklyEarnings);
        Assert.Null(_journeyState.YearlyEarnings);
    }

    [Theory]
    [InlineData(PartnerPaidWorkOption.Yes)]
    [InlineData(PartnerPaidWorkOption.ParentalLeave)]
    [InlineData(PartnerPaidWorkOption.SickLeave)]
    [InlineData(PartnerPaidWorkOption.No)]
    public void ApplyPartnerPaidWorkKeepsLaterAnswersIfOptionIsUnchanged(PartnerPaidWorkOption option)
    {
        _journeyState.PartnerPaidWork = option;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];
        _journeyState.PartnerParentalLeaveChildrenIds = ["child-a"];
        _journeyState.PartnerSelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        _journeyState.PartnerYearlyEarnings = YearlyEarningsOption.BelowThreshold;

        _journeyState.Apply(new PartnerPaidWorkViewModel { PartnerPaidWork = option });

        Assert.Equal(option, _journeyState.PartnerPaidWork);
        Assert.Equal([WorkStatusOption.SelfEmployed], _journeyState.PartnerWorkStatus);
        Assert.Equal(["child-a"], _journeyState.PartnerParentalLeaveChildrenIds);
        Assert.Equal(SelfEmployedDurationOption.NotLessThan12Months, _journeyState.PartnerSelfEmployedDuration);
        Assert.Equal(WeeklyEarningsOption.AboveThreshold, _journeyState.PartnerWeeklyEarnings);
        Assert.Equal(YearlyEarningsOption.BelowThreshold, _journeyState.PartnerYearlyEarnings);
    }

    [Fact]
    public void ApplyPartnerPaidWorkClearsParentalLeaveChildrenIfChangedToYes()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.ParentalLeave;
        _journeyState.PartnerParentalLeaveChildrenIds = ["child-a"];

        _journeyState.Apply(new PartnerPaidWorkViewModel { PartnerPaidWork = PartnerPaidWorkOption.Yes });

        Assert.Empty(_journeyState.PartnerParentalLeaveChildrenIds);
    }

    [Fact]
    public void ApplyPartnerPaidWorkClearsWeeklyEarningsIfChangedToParentalLeave()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        _journeyState.Apply(new PartnerPaidWorkViewModel { PartnerPaidWork = PartnerPaidWorkOption.ParentalLeave });

        Assert.Null(_journeyState.PartnerWeeklyEarnings);
    }

    [Fact]
    public void ApplyPartnerPaidWorkClearsLeaveAndEarningsIfChangedToSickLeave()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.ParentalLeave;
        _journeyState.PartnerParentalLeaveChildrenIds = ["child-a"];
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;

        _journeyState.Apply(new PartnerPaidWorkViewModel { PartnerPaidWork = PartnerPaidWorkOption.SickLeave });

        Assert.Empty(_journeyState.PartnerParentalLeaveChildrenIds);
        Assert.Null(_journeyState.PartnerWeeklyEarnings);
    }

    [Fact]
    public void ApplyPartnerPaidWorkClearsWorkAnswersIfChangedToNo()
    {
        _journeyState.PartnerPaidWork = PartnerPaidWorkOption.Yes;
        _journeyState.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];
        _journeyState.PartnerParentalLeaveChildrenIds = ["child-a"];
        _journeyState.PartnerSelfEmployedDuration = SelfEmployedDurationOption.NotLessThan12Months;
        _journeyState.PartnerWeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        _journeyState.PartnerYearlyEarnings = YearlyEarningsOption.BelowThreshold;

        _journeyState.Apply(new PartnerPaidWorkViewModel { PartnerPaidWork = PartnerPaidWorkOption.No });

        Assert.Empty(_journeyState.PartnerWorkStatus);
        Assert.Empty(_journeyState.PartnerParentalLeaveChildrenIds);
        Assert.Null(_journeyState.PartnerSelfEmployedDuration);
        Assert.Null(_journeyState.PartnerWeeklyEarnings);
        Assert.Null(_journeyState.PartnerYearlyEarnings);
    }
}
