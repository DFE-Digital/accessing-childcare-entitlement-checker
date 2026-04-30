using System.Globalization;
using AccessingChildcareEntitlementChecker.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
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

var app = builder.Build();
if (app.Environment.IsDevelopment())
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
