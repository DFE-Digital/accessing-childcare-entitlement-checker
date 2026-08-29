using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Models.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using Microsoft.FeatureManagement;

namespace Dfe.Acec.Web.Services.Summary;

public class UserSummaryBuilder(
    IModelMetadataProvider metadataProvider,
    IStringLocalizerFactory stringLocalizerFactory,
    IFeatureManager featureManager)
    : IUserSummaryBuilder
{
    public async Task<IReadOnlyList<SummaryRowViewModel>> BuildUserSummaryAsync(JourneyState journeyState)
    {
        var homeBuilder = new SummaryRowFactory(
            metadataProvider,
            "Home",
            stringLocalizerFactory);

        if (!await featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration))
        {
            homeBuilder.AddLocation(journeyState.CountryOfResidence);
        }

        var userBuilder = new SummaryRowFactory(metadataProvider, "User", stringLocalizerFactory)
            .AddUserAge(journeyState.UserAge)
            .Add((NationalityViewModel m) => m.Nationality, journeyState.Nationality, nameof(UserController.Nationality))
            .Add((SettledStatusViewModel m) => m.SettledStatus, journeyState.SettledStatus, nameof(UserController.SettledStatus))
            .Add((PaidWorkViewModel m) => m.PaidWork, journeyState.PaidWork, nameof(UserController.PaidWork))
            .AddParentalLeave(journeyState)
            .Add((WorkStatusViewModel m) => m.WorkStatus, journeyState.WorkStatus, nameof(UserController.WorkStatus))
            .Add((SelfEmployedDurationViewModel m) => m.SelfEmployedDuration, journeyState.SelfEmployedDuration, nameof(UserController.SelfEmployedDuration))
            .AddWeeklyEarnings(journeyState)
            .Add((YearlyEarningsViewModel m) => m.YearlyEarnings, journeyState.YearlyEarnings, nameof(UserController.YearlyEarnings))
            .Add((UniversalCreditViewModel m) => m.UniversalCredit, journeyState.UniversalCredit, nameof(UserController.UniversalCredit))
            .Add((BenefitsViewModel m) => m.Benefits, journeyState.Benefits, nameof(UserController.Benefits))
            .Add((ChildcareSupportViewModel m) => m.ChildcareSupport, journeyState.ChildcareSupport, nameof(UserController.ChildcareSupport))
            .Add((ChildcareVoucherReceiptViewModel m) => m.ChildcareVoucherReceipt, journeyState.ChildcareVoucherReceipt, nameof(UserController.ChildcareVoucherReceipt))
            .AddHasPartner(journeyState.HasPartner);

        return homeBuilder.ViewModels.Concat(userBuilder.ViewModels).ToList().AsReadOnly();
    }
}
