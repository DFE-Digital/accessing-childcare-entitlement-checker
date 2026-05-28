using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerChildcareVoucherReceiptViewModel
{
    public string? ReturnTo { get; set; }

    public PartnerChildcareVoucherReceiptViewModel()
    {
    }

    public PartnerChildcareVoucherReceiptViewModel(JourneyState journeyState)
    {
        PartnerChildcareVoucherReceipt = journeyState.Partner.PartnerChildcareVoucherReceipt;
    }

    [Display(Name = "How does your partner receive childcare vouchers?")]
    [Required(ErrorMessage = "Select how your partner receives their childcare vouchers")]
    public ChildcareVoucherReceiptOption? PartnerChildcareVoucherReceipt { get; set; }
}
