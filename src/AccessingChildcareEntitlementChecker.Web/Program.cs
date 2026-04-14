using System.Globalization;
using AccessingChildcareEntitlementChecker.Web;
using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddLocalization(options => options.ResourcesPath = "Resources")
    .AddDistributedMemoryCache()
    .AddSession()
    .AddHttpContextAccessor()
    .AddScoped<IJourneySession, JourneySession>()
    .AddGovUkFrontend();

builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        })
    .AddViewLocalization();

builder.Services.AddHealthChecks();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDevelopmentAuth(builder.Configuration);
    app.MapRobotsExclusionProtocol();
}
else
{
    app.UseExceptionHandler("/Entitlement/Error")
        .UseHsts();
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
    .UseExceptionHandler("/Error");

app.MapHealthChecks("/health");
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Start}/{id?}");

await app.RunAsync();
