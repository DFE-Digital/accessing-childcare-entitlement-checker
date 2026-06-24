using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerChildcareVoucherReceiptViewModel
{
    public PartnerChildcareVoucherReceiptViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerChildcareVoucherReceiptViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerChildcareVoucherReceipt = journeyState.PartnerChildcareVoucherReceipt;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "How does your partner receive childcare vouchers?")]
    [Required(ErrorMessage = "Select how your partner receives their childcare vouchers")]
    public ChildcareVoucherReceiptOption? PartnerChildcareVoucherReceipt { get; set; }
}
