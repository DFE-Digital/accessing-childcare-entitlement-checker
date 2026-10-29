using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Web.Tests.Unit.Services;

public class ChildChangeDestinationValidatorTests
{
    [Theory]
    [InlineData(BirthStatus.Born, BornChildDetailsController.Name, nameof(BornChildDetailsController.ChildBirthDate), true)]
    [InlineData(BirthStatus.Born, BornChildDetailsController.Name, nameof(BornChildDetailsController.ChildSupport), true)]
    [InlineData(BirthStatus.Due, ExpectedChildDetailsController.Name, nameof(ExpectedChildDetailsController.ChildDueDate), true)]
    [InlineData(BirthStatus.Born, ExpectedChildDetailsController.Name, nameof(ExpectedChildDetailsController.ChildDueDate), false)]
    [InlineData(BirthStatus.Due, BornChildDetailsController.Name, nameof(BornChildDetailsController.ChildBirthDate), false)]
    [InlineData(BirthStatus.Born, "InvalidController", "InvalidAction", false)]
    public void IsValidReturnsExpectedResult(BirthStatus birthStatus, string controller, string action, bool expected)
    {
        var result = ChildChangeDestinationValidator.IsValid(
            birthStatus,
            controller,
            action);

        Assert.Equal(expected, result);
    }
}
