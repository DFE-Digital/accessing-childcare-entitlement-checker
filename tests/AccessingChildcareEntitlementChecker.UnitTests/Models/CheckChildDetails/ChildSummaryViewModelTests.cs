using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Children;
using AccessingChildcareEntitlementChecker.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.CheckChildDetails;

public class ChildSummaryViewModelTests
{
    private readonly Guid _childAId;

    public ChildSummaryViewModelTests()
    {
        _childAId = Guid.Parse("00000000-0000-0000-0000-00000000000a");
    }

    [Fact]
    public void BornChildPopulatesViewModelAsExpected()
    {
        var bornChild = new ChildState(_childAId, "Child A")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 15),
            BornRelationship = Relationship.Parent,
            ChildSupportOptions = new List<ChildSupport>
            {
                ChildSupport.DisabilityLivingAllowance,
            },
        };

        var viewModel = new ChildSummaryViewModel(bornChild);
        Assert.NotNull(viewModel);
        Assert.Equal(_childAId, viewModel.ChildId);
        Assert.Equal("Child A", viewModel.Title);
        Assert.Equal(3, viewModel.Rows.Count);

        var birthDateRow = viewModel.Rows.First();
        Assert.Equal("Child A", birthDateRow.Param);
        Assert.Equal("15 January 2020", birthDateRow.Value);
        Assert.Equal("Children", birthDateRow.ChangeController);
        Assert.Equal("What is {0}'s date of birth?", birthDateRow.Key);
        Assert.Equal("ChildBirthDate", birthDateRow.ChangeAction);
        Assert.Equal(_childAId, birthDateRow.ChildId);
    }

    [Fact]
    public void DueChildPopulatesViewModelAsExpected()
    {
        var dueChild = new ChildState(_childAId, "Child A")
        {
            BirthStatus = BirthStatus.Due,
            DueDate = new DateOnly(2020, 1, 15),
            ExpectedRelationship = Relationship.Parent,
        };

        var viewModel = new ChildSummaryViewModel(dueChild);
        Assert.NotNull(viewModel);
        Assert.Equal(_childAId, viewModel.ChildId);
        Assert.Equal("Child A", viewModel.Title);
        Assert.Equal(2, viewModel.Rows.Count);

        var dueDateRow = viewModel.Rows.First();
        Assert.Equal("Child A", dueDateRow.Param);
        Assert.Equal("15 January 2020", dueDateRow.Value);
        Assert.Equal("Children", dueDateRow.ChangeController);
        Assert.Equal("What is this child's due date?", dueDateRow.Key);
        Assert.Equal("ChildDueDate", dueDateRow.ChangeAction);
        Assert.Equal(_childAId, dueDateRow.ChildId);
    }
}
