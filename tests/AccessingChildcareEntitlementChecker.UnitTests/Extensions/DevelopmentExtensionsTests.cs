using AccessingChildcareEntitlementChecker.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Extensions;

public class DevelopmentExtensionsTests
{
    private readonly IApplicationBuilder _app;
    private readonly IConfiguration _config;
    private readonly IHostEnvironment _env;
    private Func<RequestDelegate, RequestDelegate>? _registeredMiddleware;

    public DevelopmentExtensionsTests()
    {
        _app = Substitute.For<IApplicationBuilder>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var logger = Substitute.For<ILogger>();
        _config = Substitute.For<IConfiguration>();
        _env = Substitute.For<IHostEnvironment>();
        var serviceProvider = Substitute.For<IServiceProvider>();

        loggerFactory.CreateLogger(Arg.Any<string>()).Returns(logger);

        serviceProvider.GetService(typeof(ILoggerFactory)).Returns(loggerFactory);
        serviceProvider.GetService(typeof(IConfiguration)).Returns(_config);
        serviceProvider.GetService(typeof(IHostEnvironment)).Returns(_env);

        _app.ApplicationServices.Returns(serviceProvider);

        // Capture the middleware registered with Use
        _app.Use(Arg.Do<Func<RequestDelegate, RequestDelegate>>(m => _registeredMiddleware = m));
    }

    [Fact]
    public void UseDevelopmentAuthReturnsAppWhenPasswordIsNullOrEmpty()
    {
        _config["DevelopmentBasicAuthPassword"].Returns((string?)null);
        _env.EnvironmentName.Returns(Environments.Development);

        var result = _app.UseDevelopmentAuth();

        Assert.Same(_app, result);
        _app.DidNotReceive().Use(Arg.Any<Func<RequestDelegate, RequestDelegate>>());
    }

    [Fact]
    public void UseDevelopmentAuthReturnsAppWhenEnvironmentIsProduction()
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Production);

        var result = _app.UseDevelopmentAuth();

        Assert.Same(_app, result);
        _app.DidNotReceive().Use(Arg.Any<Func<RequestDelegate, RequestDelegate>>());
    }

    private async Task InvokeMiddlewareAsync(HttpContext context, RequestDelegate next)
    {
        Assert.NotNull(_registeredMiddleware);
        var middleware = _registeredMiddleware!(next);
        await middleware(context);
    }

    [Theory]
    [InlineData("/health")]
    [InlineData("/assets/manifest.json")]
    [InlineData("/assets/images/favicon.ico")]
    [InlineData("/robots.txt")]
    [InlineData("/robots933456.txt")]
    [InlineData("/ROBOTS_TEST.TXT")]
    public async Task UseDevelopmentAuthAllowsExcludedPathsWithoutAuthentication(string path)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.Method = "GET";

        var nextCalled = false;

        await InvokeMiddlewareAsync(context, Next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode); // Default status since next ran
        return;

        Task Next(HttpContext _)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }
    }

    [Theory]
    [InlineData("AlwaysOn")]
    [InlineData("SiteWarmup")]
    public async Task UseDevelopmentAuthAllowsAzureProbesWithoutAuthentication(string userAgent)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = "/";
        context.Request.Method = "GET";
        context.Request.Headers.UserAgent = userAgent;

        var nextCalled = false;

        await InvokeMiddlewareAsync(context, Next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        return;

        Task Next(HttpContext _)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/home")]
    [InlineData("/assets")] // Not ending with trailing slash, not considered asset folder path
    [InlineData("/robots")] // Not ending with .txt
    [InlineData("/robots.png")]
    public async Task UseDevelopmentAuthBlocksNonExcludedPathsWithoutAuthentication(string path)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.Method = "GET";

        var nextCalled = false;

        await InvokeMiddlewareAsync(context, Next);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.Equal("Basic realm=\"Development\"", context.Response.Headers.WWWAuthenticate.ToString());
        return;

        Task Next(HttpContext _)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task UseDevelopmentAuthAllowsCorrectCredentials()
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = "/";
        context.Request.Method = "GET";

        // base64(user:password) -> base64("admin:password") -> "YWRtaW46cGFzc3dvcmQ="
        context.Request.Headers.Authorization = "Basic YWRtaW46cGFzc3dvcmQ=";

        var nextCalled = false;

        await InvokeMiddlewareAsync(context, Next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
        return;

        Task Next(HttpContext _)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task UseDevelopmentAuthBlocksIncorrectCredentials()
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = "/";
        context.Request.Method = "GET";
        context.Request.Headers.UserAgent = "Mozilla/5.0";

        // base64("admin:wrong") -> "YWRtaW46d3Jvbmc="
        context.Request.Headers.Authorization = "Basic YWRtaW46d3Jvbmc=";

        var nextCalled = false;

        await InvokeMiddlewareAsync(context, Next);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        return;

        Task Next(HttpContext _)
        {
            nextCalled = true;
            return Task.CompletedTask;
        }
    }
}
