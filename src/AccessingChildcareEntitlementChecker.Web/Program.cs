using System.Globalization;
using AccessingChildcareEntitlementChecker.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.RulesEngine.Extensions;
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
    .AddGovUkFrontend()
    .AddHealthChecks();

services
    .AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        })
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

services.AddJourneyServices();

services.AddRulesEngine();

services.AddScoped<EntitlementRulesEngine>();

var app = builder.Build();
if (!app.Environment.IsProduction())
{
    app.UseDevelopmentAuth(builder.Configuration);
    app.MapRobotsExclusionProtocol();

    app.MapGet("/throw", () =>
    {
        throw new InvalidOperationException("Test exception for error page");
    });
}
else
{
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    context.Response.Headers.XFrameOptions = "DENY";
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

    context.Response.Headers.ContentSecurityPolicy =
        "default-src 'self'; " +
        "script-src 'self'; " +
        "style-src 'self'; " +
        "img-src 'self' data:; " +
        "font-src 'self'; " +
        "object-src 'none'; " +
        "base-uri 'self'; " +
        "frame-ancestors 'none';";

    await next();
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

app.MapHealthChecks("/health");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Start}/{id?}");

await app.RunAsync();
