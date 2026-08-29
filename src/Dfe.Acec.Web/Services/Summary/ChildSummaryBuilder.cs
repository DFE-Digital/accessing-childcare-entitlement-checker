using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Models.ExpectedChildDetails;
using Dfe.Acec.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Services.Summary;

public class ChildSummaryBuilder(
    IModelMetadataProvider metadataProvider,
    IStringLocalizerFactory stringLocalizerFactory)
    : IChildSummaryBuilder
{
    public ChildSummaryViewModel BuildChildSummary(Child child, string returnTo)
    {
        var born = new SummaryRowFactory(metadataProvider, "BornChildDetails", stringLocalizerFactory)
            .Add((ChildBirthDateViewModel m) => m.ChildBirthDate, child.BirthDate, nameof(BornChildDetailsController.ChildBirthDate))
            .Add((ChildSupportViewModel m) => m.ChildSupportOptions, child.ChildSupportOptions, nameof(BornChildDetailsController.ChildSupport));

        var expected = new SummaryRowFactory(metadataProvider, "ExpectedChildDetails", stringLocalizerFactory)
            .Add((ChildDueDateViewModel m) => m.ChildDueDate, child.DueDate, nameof(ExpectedChildDetailsController.ChildDueDate));

        var summaryRows = born.ViewModels.Concat(expected.ViewModels).ToList().AsReadOnly();
        return new ChildSummaryViewModel(child.ChildId, child.Name, returnTo, summaryRows);
    }
}
