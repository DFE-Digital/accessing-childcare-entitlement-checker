using Dfe.Acec.Web;
using GovUk.Frontend.AspNetCore;

// Prevent Redis timeouts under bursty load (e.g. E2E tests, traffic spikes)
// by explicitly setting a higher minimum for the ThreadPool.
// See: https://stackexchange.github.io/StackExchange.Redis/Timeouts#threadpool-growth
ThreadPool.SetMinThreads(200, 200);

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var securePolicy = builder.Environment.IsDevelopment()
    ? CookieSecurePolicy.SameAsRequest
    : CookieSecurePolicy.Always;

services.AddSingleton(typeof(CookieSecurePolicy), securePolicy)
    .AddSecurityConfiguration(builder.Configuration, securePolicy)
    .AddWebComponents()
    .AddTelemetryAndProxies(builder.Configuration);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseDevelopmentAuth()
    .UseSecurityHeaders()
    .UseRequestLocalizationDefaults()
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
app.MapControllerRoutes();

await app.RunAsync();
