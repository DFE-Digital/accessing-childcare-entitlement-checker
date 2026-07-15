using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AccessingChildcareEntitlementChecker.Web.Models.Cookies;

public record CookiesViewModel(
    [property: BindNever]
    bool HasSetCookies,
    [property: Required()]
    bool? AnalyticsCookiesEnabled);