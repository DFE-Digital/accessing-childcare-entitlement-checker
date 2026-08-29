using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Dfe.Acec.Web.Controllers;
using Microsoft.AspNetCore.Localization;

namespace Dfe.Acec.Web;

[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
public static class WebApplicationExtensions
{
    private const string Home = "Home";
    private const string Introduction = "Introduction";
    private const string BornChildDetails = "BornChildDetails";
    private const string ExpectedChildDetails = "ExpectedChildDetails";
    private const string User = "User";
    private const string Partner = "Partner";
    private const string Summary = "Summary";
    private const string Results = "Results";
    private const string Error = "Error";

    public static WebApplication MapControllerRoutes(this WebApplication app)
    {
        app.MapControllerRoute(name: "root", pattern: "", defaults: new { controller = Home, action = nameof(HomeController.Start) });
        app.MapControllerRoute(name: "start-page", pattern: "start-page", defaults: new { controller = Home, action = nameof(HomeController.Start) });
        app.MapControllerRoute(name: "session-expired", pattern: "session-expired", defaults: new { controller = Home, action = nameof(HomeController.SessionExpired) });
        app.MapControllerRoute(name: "where-do-you-live", pattern: "where-do-you-live", defaults: new { controller = Home, action = nameof(HomeController.Location) });

        app.MapControllerRoute(name: "children-add-child-details-childId", pattern: "children/add-child-details/{childId?}", defaults: new { controller = Introduction, action = nameof(IntroductionController.ChildName) });
        app.MapControllerRoute(name: "children-childId-has-the-child-been-born", pattern: "children/{childId}/has-the-child-been-born", defaults: new { controller = Introduction, action = nameof(IntroductionController.IsChildBorn) });

        app.MapControllerRoute(name: "children-childId-childs-date-of-birth", pattern: "children/{childId}/childs-date-of-birth", defaults: new { controller = BornChildDetails, action = nameof(BornChildDetailsController.ChildBirthDate) });
        app.MapControllerRoute(name: "children-childId-child-benefits", pattern: "children/{childId}/child-benefits", defaults: new { controller = BornChildDetails, action = nameof(BornChildDetailsController.ChildSupport) });

        app.MapControllerRoute(name: "children-childId-expectant-childs-due-date", pattern: "children/{childId}/expectant-childs-due-date", defaults: new { controller = ExpectedChildDetails, action = nameof(ExpectedChildDetailsController.ChildDueDate) });

        app.MapControllerRoute(name: "children-check-childs-details", pattern: "children/check-childs-details", defaults: new { controller = Summary, action = nameof(SummaryController.CheckChildDetails) });
        app.MapControllerRoute(name: "children-childId-remove", pattern: "children/{childId?}/remove", defaults: new { controller = Summary, action = nameof(SummaryController.Remove) });

        app.MapControllerRoute(name: "age-parent-age", pattern: "age/parent-age", defaults: new { controller = User, action = nameof(UserController.UserAge) });
        // N.b. shown as "age/parent-age" on the lucid...
        app.MapControllerRoute(name: "nationality", pattern: "nationality", defaults: new { controller = User, action = nameof(UserController.Nationality) });
        app.MapControllerRoute(name: "nationality-settled-status", pattern: "nationality/settled-status", defaults: new { controller = User, action = nameof(UserController.SettledStatus) });
        app.MapControllerRoute(name: "work-status-work", pattern: "work-status/work", defaults: new { controller = User, action = nameof(UserController.PaidWork) });
        app.MapControllerRoute(name: "work-status-work-status", pattern: "work-status/work-status", defaults: new { controller = User, action = nameof(UserController.WorkStatus) });
        // N.b. undefined on the lucid - pending design
        app.MapControllerRoute(name: "leave-parental-leave", pattern: "leave/parental-leave", defaults: new { controller = User, action = nameof(UserController.ParentalLeave) });
        app.MapControllerRoute(name: "work-status-self-employed", pattern: "work-status/self-employed", defaults: new { controller = User, action = nameof(UserController.SelfEmployedDuration) });
        app.MapControllerRoute(name: "earnings-adjusted-net-income", pattern: "earnings/adjusted-net-income", defaults: new { controller = User, action = nameof(UserController.YearlyEarnings) });
        app.MapControllerRoute(name: "earnings-wage", pattern: "earnings/wage", defaults: new { controller = User, action = nameof(UserController.WeeklyEarnings) });
        app.MapControllerRoute(name: "benefits-universal-credit", pattern: "benefits/universal-credit", defaults: new { controller = User, action = nameof(UserController.UniversalCredit) });
        app.MapControllerRoute(name: "benefits-benefits", pattern: "benefits/benefits", defaults: new { controller = User, action = nameof(UserController.Benefits) });
        app.MapControllerRoute(name: "benefits-childcare-support", pattern: "benefits/childcare-support", defaults: new { controller = User, action = nameof(UserController.ChildcareSupport) });
        app.MapControllerRoute(name: "benefits-childcare-vouchers", pattern: "benefits/childcare-vouchers", defaults: new { controller = User, action = nameof(UserController.ChildcareVoucherReceipt) });
        app.MapControllerRoute(name: "partner", pattern: "partner", defaults: new { controller = User, action = nameof(UserController.HasPartner) });

        app.MapControllerRoute(name: "age-partner-age", pattern: "age/partner-age", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerAge) });
        app.MapControllerRoute(name: "nationality-nationality-partner", pattern: "nationality/nationality-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerNationality) });
        app.MapControllerRoute(name: "nationality-settled-status-partner", pattern: "nationality/settled-status-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerSettledStatus) });
        app.MapControllerRoute(name: "work-status-work-partner", pattern: "work-status/work-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerPaidWork) });
        app.MapControllerRoute(name: "leave-partner-parental-leave", pattern: "leave/parental-leave-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerParentalLeave) });
        app.MapControllerRoute(name: "work-status-work-status-partner", pattern: "work-status/work-status-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerWorkStatus) });
        app.MapControllerRoute(name: "Partner-PartnerBenefits", pattern: "Partner/PartnerBenefits", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerBenefits) });
        // N.b. undefined on the lucid from here on out
        app.MapControllerRoute(name: "work-status-self-employed-partner", pattern: "work-status/self-employed-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerSelfEmployedDuration) });
        app.MapControllerRoute(name: "earnings-wage-partner", pattern: "earnings/wage-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerWeeklyEarnings) });
        app.MapControllerRoute(name: "earnings-adjusted-net-income-partner", pattern: "earnings/adjusted-net-income-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerYearlyEarnings) });
        app.MapControllerRoute(name: "benefits-childcare-support-partner", pattern: "benefits/childcare-support-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerChildcareSupport) });
        app.MapControllerRoute(name: "benefits-childcare-vouchers-partner", pattern: "benefits/childcare-vouchers-partner", defaults: new { controller = Partner, action = nameof(PartnerController.PartnerChildcareVoucherReceipt) });

        app.MapControllerRoute(name: "check-your-answers", pattern: "check-your-answers", defaults: new { controller = Summary, action = nameof(SummaryController.CheckAnswers) });

        app.MapControllerRoute(name: "results", pattern: "results", defaults: new { controller = Results, action = nameof(ResultsController.Results) });

        app.MapControllerRoute(name: "Error", pattern: "Error", defaults: new { controller = Error, action = nameof(ErrorController.InternalServerError) });
        app.MapControllerRoute(name: "Error-statusCode-int", pattern: "Error/{statusCode:int}", defaults: new { controller = Error, action = nameof(ErrorController.StatusCodePage) });
        app.MapControllerRoute(name: "Cookies", pattern: "cookies", defaults: new { controller = CookiesController.Name, action = nameof(CookiesController.Cookies) });

        app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Start}/{id?}", defaults: new { });

        return app;
    }

    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var bytes = RandomNumberGenerator.GetBytes(12);
            context.Items["csp-nonce"] = Convert.ToBase64String(bytes);

            context.Response.Headers.XFrameOptions = "DENY";
            context.Response.Headers.XContentTypeOptions = "nosniff";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            var csp = new StringBuilder();

            csp.Append("default-src 'self'; ");
            csp.Append($"script-src 'self' 'nonce-{context.Items["csp-nonce"]}' " +
                       "https://www.googletagmanager.com " +
                       "https://*.clarity.ms; ");
            csp.Append("style-src 'self'; ");
            csp.Append("img-src 'self' data: https://*.google-analytics.com https://www.googletagmanager.com; ");
            csp.Append("font-src 'self'; ");
            csp.Append(
                "connect-src 'self' " +
                "https://www.google-analytics.com " +
                "https://*.google-analytics.com " +
                "https://www.googletagmanager.com " +
                "https://*.clarity.ms; ");
            csp.Append("frame-src https://www.googletagmanager.com; ");
            csp.Append("form-action 'self'; ");
            csp.Append("object-src 'none'; ");
            csp.Append("base-uri 'self'; ");
            csp.Append("frame-ancestors 'none'; ");

            context.Response.Headers.ContentSecurityPolicy = csp.ToString();

            await next(context);
        });

        return app;
    }

    public static IApplicationBuilder UseRequestLocalizationDefaults(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { new CultureInfo("en-GB") };
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en-GB"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures
        });

        return app;
    }
}
