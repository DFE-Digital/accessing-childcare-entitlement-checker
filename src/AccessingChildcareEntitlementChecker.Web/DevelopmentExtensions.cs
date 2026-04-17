using System.Text;

namespace AccessingChildcareEntitlementChecker.Web;

public static class DevelopmentExtensions
{
    private const string DevelopmentBasicAuthPasswordSettingName = "DevelopmentBasicAuthPassword";

    public static IApplicationBuilder UseDevelopmentAuth(this IApplicationBuilder app, IConfiguration configuration)
    {
        var developmentBasicAuthPassword = configuration[DevelopmentBasicAuthPasswordSettingName]
            ?? throw new InvalidOperationException($"{DevelopmentBasicAuthPasswordSettingName} must be configured in Development.");

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.Equals("/health", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.Equals("/robots.txt", StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            var authorizationHeader = context.Request.Headers.Authorization.ToString();
            const string basicPrefix = "Basic ";

            if (!authorizationHeader.StartsWith(basicPrefix, StringComparison.OrdinalIgnoreCase))
            {
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
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.Headers.WWWAuthenticate = "Basic realm=\"Development\"";
                    return;
                }
            }
            catch (FormatException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers.WWWAuthenticate = "Basic realm=\"Development\"";
                return;
            }

            await next();
        });

        return app;
    }

    public static IEndpointRouteBuilder MapRobotsExclusionProtocol(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/robots.txt", () => Results.Text("User-agent: *\nDisallow: /", "text/plain"));

        return endpoints;
    }
}
