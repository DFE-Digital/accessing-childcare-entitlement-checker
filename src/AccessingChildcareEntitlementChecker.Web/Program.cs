using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using AccessingChildcareEntitlementChecker.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.RulesEngine.Extensions;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using Azure.Monitor.OpenTelemetry.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var appInsightsConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (!string.IsNullOrEmpty(appInsightsConnectionString))
{
    services.AddOpenTelemetry().UseAzureMonitor();
}

services
    .AddLocalization(options => options.ResourcesPath = "Resources")
    .AddDistributedMemoryCache()
    .AddSession()
    .AddHttpContextAccessor()
    .AddGovUkFrontend(options =>
    {
        options.GetCspNonceForRequest = context => context.Items["csp-nonce"]?.ToString();
    })
    .AddHealthChecks();

services
    .AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        })
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

services.AddJourneyServices();

services.AddScoped<JourneyStateToEntitlementRequestMapper>();

services.AddRulesEngine();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    //
}
else
{
    app.UseHsts();
}

app.UseDevelopmentAuth();
app.Use(async (context, next) =>
{
    var bytes = RandomNumberGenerator.GetBytes(12);
    context.Items["csp-nonce"] = Convert.ToBase64String(bytes);

    context.Response.Headers.XFrameOptions = "DENY";
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    var csp = new StringBuilder();
    csp.Append("default-src 'self'; ");
    csp.Append($"script-src 'self' 'nonce-{context.Items["csp-nonce"]}'; ");
    csp.Append("style-src 'self'; ");
    csp.Append("img-src 'self' data:; ");
    csp.Append("font-src 'self'; ");
    csp.Append("connect-src 'self'; ");
    csp.Append("form-action 'self'; ");
    csp.Append("object-src 'none'; ");
    csp.Append("base-uri 'self'; ");
    csp.Append("frame-ancestors 'none'; ");

    context.Response.Headers.ContentSecurityPolicy = csp.ToString();

    await next(context);
});

var supportedCultures = new[] { new CultureInfo("en-GB") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
})
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseGovUkFrontend()
    .UseRouting()
    .UseSession()
    .UseAuthorization()
    .UseExceptionHandler("/Error")
    .UseStatusCodePagesWithReExecute("/error/{0}");

app.MapTestException();
app.MapRobotsExclusionProtocol();
app.MapHealthChecks("/health");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Start}/{id?}");

await app.RunAsync();
