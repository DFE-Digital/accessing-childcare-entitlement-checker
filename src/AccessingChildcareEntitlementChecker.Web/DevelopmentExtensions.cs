using System.Text;

namespace AccessingChildcareEntitlementChecker.Web;

public static class DevelopmentExtensions
{
    private const string DevelopmentBasicAuthPasswordSettingName = "DevelopmentBasicAuthPassword";

    public static IApplicationBuilder UseDevelopmentAuth(this IApplicationBuilder app)
    {
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>()
            .CreateLogger(typeof(DevelopmentExtensions));
        var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();

        var developmentBasicAuthPassword = config[DevelopmentBasicAuthPasswordSettingName];

        if (string.IsNullOrEmpty(developmentBasicAuthPassword) || env.IsProduction())
        {
            return app;
        }

        app.Use(async (context, next) =>
        {
            var path = context.Request.Path.Value ?? string.Empty;

            if (IsExcludedPath(path) || IsAlwaysOnOrWarmupProbe(context, path))
            {
                await next();
                return;
            }

            var authorizationHeader = context.Request.Headers.Authorization.ToString();
            const string basicPrefix = "Basic ";

            if (!authorizationHeader.StartsWith(basicPrefix, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogWarning(
                    "Development auth failed: Missing or invalid Authorization header format. Method: {Method}, Path: {Path}, UserAgent: {UserAgent}",
                    context.Request.Method, context.Request.Path, context.Request.Headers.UserAgent);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.WWWAuthenticate = "Basic realm=\"Development\"";
                return;
            }

            try
            {
                var encodedCredentials = authorizationHeader[basicPrefix.Length..];
                var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var separatorIndex = decodedCredentials.IndexOf(':');
                var password = separatorIndex >= 0 ? decodedCredentials[(separatorIndex + 1)..] : string.Empty;

                if (!string.Equals(password, developmentBasicAuthPassword, StringComparison.Ordinal))
                {
                    logger.LogWarning(
                        "Development auth failed: Incorrect password provided. Method: {Method}, Path: {Path}, UserAgent: {UserAgent}",
                        context.Request.Method, context.Request.Path, context.Request.Headers.UserAgent);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Headers.WWWAuthenticate = "Basic realm=\"Development\"";
                    return;
                }
            }
            catch (FormatException ex)
            {
                logger.LogWarning(ex,
                    "Development auth failed: Malformed credentials format. Method: {Method}, Path: {Path}, UserAgent: {UserAgent}",
                    context.Request.Method, context.Request.Path, context.Request.Headers.UserAgent);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.WWWAuthenticate = "Basic realm=\"Development\"";
                return;
            }

            await next();
        });

        return app;
    }

    private static bool IsExcludedPath(string path) =>
        IsHealth(path) ||
        IsAssets(path) ||
        IsRobotsTxt(path);

    private static bool IsAlwaysOnOrWarmupProbe(HttpContext context, string path) =>
        string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) &&
        path.Equals("/", StringComparison.OrdinalIgnoreCase) &&
        (string.Equals(context.Request.Headers.UserAgent.ToString(), "AlwaysOn", StringComparison.OrdinalIgnoreCase) ||
         string.Equals(context.Request.Headers.UserAgent.ToString(), "SiteWarmup", StringComparison.OrdinalIgnoreCase));

    private static bool IsHealth(string path) =>
        path.Equals("/health", StringComparison.OrdinalIgnoreCase);

    private static bool IsAssets(string path) =>
        path.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase);

    private static bool IsRobotsTxt(string path) =>
        path.StartsWith("/robots", StringComparison.OrdinalIgnoreCase) &&
        path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);

    public static IEndpointRouteBuilder MapRobotsExclusionProtocol(this IEndpointRouteBuilder builder)
    {
        var env = builder.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (!env.IsProduction())
        {
            builder.MapGet("/robots.txt", () => Results.Text("User-agent: *\nDisallow: /", "text/plain"));
        }

        return builder;
    }

    public static IEndpointRouteBuilder MapTestException(this IEndpointRouteBuilder builder)
    {
        var env = builder.ServiceProvider.GetRequiredService<IHostEnvironment>();

        if (!env.IsProduction())
        {
            builder.MapGet("/throw", () => { throw new InvalidOperationException("Test exception for error page"); });
        }

        return builder;
    }
}