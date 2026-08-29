using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Services.Summary;

public class PartnerSummaryBuilder(
    IModelMetadataProvider metadataProvider,
    IStringLocalizerFactory stringLocalizerFactory)
    : IPartnerSummaryBuilder
{
    public IReadOnlyList<SummaryRowViewModel> BuildPartnerSummary(JourneyState journeyState)
    {
        var partnerBuilder = new SummaryRowFactory(metadataProvider, "Partner", stringLocalizerFactory)
            .AddPartnerAge(journeyState.PartnerAge)
            .Add((PartnerNationalityViewModel m) => m.PartnerNationality, journeyState.PartnerNationality, nameof(PartnerController.PartnerNationality))
            .Add((PartnerSettledStatusViewModel m) => m.PartnerSettledStatus, journeyState.PartnerSettledStatus, nameof(PartnerController.PartnerSettledStatus))
            .Add((PartnerPaidWorkViewModel m) => m.PartnerPaidWork, journeyState.PartnerPaidWork, nameof(PartnerController.PartnerPaidWork))
            .AddPartnerParentalLeave(journeyState)
            .Add((PartnerWorkStatusViewModel m) => m.PartnerWorkStatus, journeyState.PartnerWorkStatus, nameof(PartnerController.PartnerWorkStatus))
            .Add((PartnerSelfEmployedDurationViewModel m) => m.PartnerSelfEmployedDuration, journeyState.PartnerSelfEmployedDuration, nameof(PartnerController.PartnerSelfEmployedDuration))
            .AddPartnerWeeklyEarnings(journeyState)
            .Add((PartnerYearlyEarningsViewModel m) => m.PartnerYearlyEarnings, journeyState.PartnerYearlyEarnings, nameof(PartnerController.PartnerYearlyEarnings))
            .Add((PartnerBenefitsViewModel m) => m.PartnerBenefits, journeyState.PartnerBenefits, nameof(PartnerController.PartnerBenefits))
            .Add((PartnerChildcareSupportViewModel m) => m.PartnerChildcareSupport, journeyState.PartnerChildcareSupport, nameof(PartnerController.PartnerChildcareSupport))
            .Add((PartnerChildcareVoucherReceiptViewModel m) => m.PartnerChildcareVoucherReceipt, journeyState.PartnerChildcareVoucherReceipt, nameof(PartnerController.PartnerChildcareVoucherReceipt));

        return partnerBuilder.ViewModels;
    }
}
