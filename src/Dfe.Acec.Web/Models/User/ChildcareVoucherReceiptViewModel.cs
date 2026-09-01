using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models.User;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ChildcareVoucherReceiptViewModel
{
    public ChildcareVoucherReceiptViewModel()
    {
        BackLink = string.Empty;
    }

    public ChildcareVoucherReceiptViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        ChildcareVoucherReceipt = journeyState.ChildcareVoucherReceipt;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [Display(Name = "How do you receive your childcare vouchers?")]
    [Required(ErrorMessage = "Select how you receive your childcare vouchers")]
    public ChildcareVoucherReceiptOption? ChildcareVoucherReceipt { get; set; }
}
