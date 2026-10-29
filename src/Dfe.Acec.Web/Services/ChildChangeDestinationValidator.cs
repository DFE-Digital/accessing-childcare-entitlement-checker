using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;

namespace Dfe.Acec.Web.Services;

public static class ChildChangeDestinationValidator
{
    public static bool IsValid(BirthStatus? birthStatus, string targetController, string targetAction)
    {
        return (birthStatus, targetController, targetAction) switch
        {
            (BirthStatus.Born, BornChildDetailsController.Name, nameof(BornChildDetailsController.ChildBirthDate)) => true,

            (BirthStatus.Born, BornChildDetailsController.Name, nameof(BornChildDetailsController.ChildSupport)) => true,

            (BirthStatus.Due, ExpectedChildDetailsController.Name, nameof(ExpectedChildDetailsController.ChildDueDate)) => true,

            _ => false
        };
    }
}
