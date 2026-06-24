using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

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
