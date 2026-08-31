using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models.Partner;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
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
