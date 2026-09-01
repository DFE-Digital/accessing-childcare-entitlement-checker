using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dfe.Acec.Web.Models.Cookies;

public record CookiesViewModel(
    [BindNever]
    bool? HasSetCookies,
    [Required]
    bool? AnalyticsCookiesEnabled);
