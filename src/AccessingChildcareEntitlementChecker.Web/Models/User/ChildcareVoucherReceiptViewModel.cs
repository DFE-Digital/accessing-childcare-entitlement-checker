using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class ChildcareVoucherReceiptViewModel
{
    public string? ReturnTo { get; set; }

    public ChildcareVoucherReceiptViewModel()
    {
    }

    public ChildcareVoucherReceiptViewModel(JourneyState journeyState)
    {
        ChildcareVoucherReceipt = journeyState.User.ChildcareVoucherReceipt;
    }

    [Display(Name = "How do you receive your childcare vouchers?")]
    [Required(ErrorMessage = "Select how you receive your childcare vouchers")]
    public ChildcareVoucherReceiptOption? ChildcareVoucherReceipt { get; set; }
}
