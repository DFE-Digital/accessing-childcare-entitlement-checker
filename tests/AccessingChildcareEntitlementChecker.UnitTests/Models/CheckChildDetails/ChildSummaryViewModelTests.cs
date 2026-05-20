using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models.CheckChildDetails;

public class ChildSummaryViewModelTests
{
    public ChildSummaryViewModelTests()
    {

    }

    [Fact]
    public void BornChildPopulatesViewModelAsExpected()
    {
        var bornChild = new Child("child-a", "Child A")
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
        Assert.Equal("child-a", viewModel.ChildId);
        Assert.Equal("Child A", viewModel.Title);
        Assert.Equal(3, viewModel.Rows.Count);

        var birthDateRow = viewModel.Rows.First();
        Assert.Equal("Child A", birthDateRow.Param);
        Assert.Equal("15 January 2020", birthDateRow.Value);
        Assert.Equal("BornChildDetails", birthDateRow.ChangeController);
        Assert.Equal("What is {0}'s date of birth?", birthDateRow.Key);
        Assert.Equal("ChildBirthDate", birthDateRow.ChangeAction);
        Assert.Equal("child-a", birthDateRow.ChildId);
    }

    [Fact]
    public void DueChildPopulatesViewModelAsExpected()
    {
        var dueChild = new Child("child-a", "Child A")
        {
            BirthStatus = BirthStatus.Due,
            DueDate = new DateOnly(2020, 1, 15),
            ExpectedRelationship = Relationship.Parent,
        };

        var viewModel = new ChildSummaryViewModel(dueChild);
        Assert.NotNull(viewModel);
        Assert.Equal("child-a", viewModel.ChildId);
        Assert.Equal("Child A", viewModel.Title);
        Assert.Equal(2, viewModel.Rows.Count);

        var dueDateRow = viewModel.Rows.First();
        Assert.Equal("Child A", dueDateRow.Param);
        Assert.Equal("15 January 2020", dueDateRow.Value);
        Assert.Equal("ExpectedChildDetails", dueDateRow.ChangeController);
        Assert.Equal("What is this child's due date?", dueDateRow.Key);
        Assert.Equal("ChildDueDate", dueDateRow.ChangeAction);
        Assert.Equal("child-a", dueDateRow.ChildId);
    }
}
