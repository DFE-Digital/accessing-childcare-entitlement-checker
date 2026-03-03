using System.Globalization;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddLocalization(options => options.ResourcesPath = "Resources")
    .AddControllersWithViews(options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    })
    .AddViewLocalization();

builder.Services
    .AddDistributedMemoryCache()
    .AddSession()
    .AddHttpContextAccessor()
    .AddScoped<IJourneySession, JourneySession>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Entitlement/Error");
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
    .UseRouting()
    .UseSession()
    .UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Entitlement}/{action=WhereDoYouLive}/{id?}");

app.Run();