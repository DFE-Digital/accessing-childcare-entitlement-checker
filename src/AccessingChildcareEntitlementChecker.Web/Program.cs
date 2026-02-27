using System.Globalization;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    })
    .AddViewLocalization();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJourneySession, JourneySession>();

var app = builder.Build();

var supportedCultures = new[] { new CultureInfo("en-GB") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Entitlement/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Entitlement}/{action=WhereDoYouLive}/{id?}");

app.Run();