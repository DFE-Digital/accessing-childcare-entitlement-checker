using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Partner;
using Dfe.Acec.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Web.Services.Summary;

public class SummaryViewModelBuilder(
    IUserSummaryBuilder userSummaryBuilder,
    IPartnerSummaryBuilder partnerSummaryBuilder,
    IChildSummaryBuilder childSummaryBuilder)
    : ISummaryViewModelBuilder
{
    public CheckChildDetailsViewModel BuildCheckChildDetailsViewModel(
        JourneyState journeyState,
        IUrlHelper urlHelper,
        string? childId = null,
        IReadOnlyList<string>? removedChildNames = null)
    {
        var summaries = journeyState.Children.Values
            .Select(child => childSummaryBuilder.BuildChildSummary(child, ReturnTo.CheckChildDetails))
            .ToList()
            .AsReadOnly();

        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, childId);
        var backLink = GetCheckChildDetailsBackLink(lastEditedChild, urlHelper);

        return new CheckChildDetailsViewModel(
            summaries,
            hasChildren,
            lastEditedChild,
            backLink,
            journeyState.CorrelationId,
            removedChildNames ?? []);
    }

    public async Task<CheckAnswersViewModel> BuildCheckAnswersViewModelAsync(
        JourneyState journeyState,
        IUrlHelper urlHelper,
        string? fromChildId = null,
        IReadOnlyList<string>? removedChildNames = null)
    {
        var summaries = journeyState.Children.Values
            .Select(child => childSummaryBuilder.BuildChildSummary(child, ReturnTo.CheckAnswers))
            .ToList()
            .AsReadOnly();

        var hasChildren = journeyState.Children.Count > 0;
        var lastEditedChild = ResolveLastEditedChild(journeyState, fromChildId);

        var userDetails = await userSummaryBuilder.BuildUserSummaryAsync(journeyState);
        var partnerDetails = partnerSummaryBuilder.BuildPartnerSummary(journeyState);
        var backLink = GetCheckAnswersBackLink(journeyState, urlHelper);

        return new CheckAnswersViewModel(
            summaries,
            hasChildren,
            lastEditedChild,
            userDetails,
            partnerDetails,
            backLink,
            journeyState.CorrelationId,
            removedChildNames ?? []);
    }

    private static Child? ResolveLastEditedChild(JourneyState journeyState, string? childId)
    {
        if (childId is not null && journeyState.Children.TryGetValue(childId, out var child))
        {
            return child;
        }

        return journeyState.Children.Values.LastOrDefault();
    }

    private static string GetCheckChildDetailsBackLink(Child? child, IUrlHelper urlHelper)
    {
        if (child?.BirthStatus == BirthStatus.Born)
        {
            return urlHelper.ActionOrThrow(nameof(BornChildDetailsController.ChildSupport), BornChildDetailsController.Name, new { childId = child.ChildId });
        }

        if (child?.BirthStatus == BirthStatus.Due)
        {
            return urlHelper.ActionOrThrow(nameof(ExpectedChildDetailsController.ChildDueDate), ExpectedChildDetailsController.Name, new { childId = child.ChildId });
        }

        return urlHelper.ActionOrThrow(nameof(IntroductionController.ChildName), IntroductionController.Name);
    }

    private static string GetCheckAnswersBackLink(JourneyState journeyState, IUrlHelper urlHelper)
    {
        if (journeyState.HasPartner!.Value)
        {
            if (journeyState.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers))
            {
                return urlHelper.ActionOrThrow(nameof(PartnerController.PartnerChildcareVoucherReceipt), PartnerController.Name);
            }

            return urlHelper.ActionOrThrow(nameof(PartnerController.PartnerChildcareSupport), PartnerController.Name);
        }

        return urlHelper.ActionOrThrow(nameof(UserController.HasPartner), UserController.Name);
    }
}
