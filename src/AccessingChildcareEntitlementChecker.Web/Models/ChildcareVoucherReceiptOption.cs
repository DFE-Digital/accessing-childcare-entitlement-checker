using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public enum ChildcareVoucherReceiptOption
{
    [Display(Name = "A workplace nursery scheme", Description = "Your employer provides childcare places directly, for example through a nursery linked to your workplace")]
    WorkplaceNurseryScheme,

    [Display(Name = "Your employer arranges with a provider", Description = "Your employer pays a childcare provider directly for you, rather than giving you vouchers")]
    EmployerArrangesWithProvider,

    [Display(Name = "Through a salary sacrifice scheme", Description = "Vouchers are taken from your pay before tax, often shown on your payslip")]
    ThroughSalarySacrifice,
}