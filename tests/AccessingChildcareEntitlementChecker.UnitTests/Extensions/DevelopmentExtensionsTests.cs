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
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    private readonly IHostEnvironment _env;
    private readonly IServiceProvider _serviceProvider;
    private Func<RequestDelegate, RequestDelegate>? _registeredMiddleware;

    public DevelopmentExtensionsTests()
    {
        _app = Substitute.For<IApplicationBuilder>();
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _logger = Substitute.For<ILogger>();
        _config = Substitute.For<IConfiguration>();
        _env = Substitute.For<IHostEnvironment>();
        _serviceProvider = Substitute.For<IServiceProvider>();

        _loggerFactory.CreateLogger(Arg.Any<string>()).Returns(_logger);

        _serviceProvider.GetService(typeof(ILoggerFactory)).Returns(_loggerFactory);
        _serviceProvider.GetService(typeof(IConfiguration)).Returns(_config);
        _serviceProvider.GetService(typeof(IHostEnvironment)).Returns(_env);

        _app.ApplicationServices.Returns(_serviceProvider);

        // Capture the middleware registered with Use
        _app.Use(Arg.Do<Func<RequestDelegate, RequestDelegate>>(m => _registeredMiddleware = m));
    }

    [Fact]
    public void UseDevelopmentAuth_ReturnsApp_When_Password_Is_Null_Or_Empty()
    {
        _config["DevelopmentBasicAuthPassword"].Returns((string?)null);
        _env.EnvironmentName.Returns(Environments.Development);

        var result = _app.UseDevelopmentAuth();

        Assert.Same(_app, result);
        _app.DidNotReceive().Use(Arg.Any<Func<RequestDelegate, RequestDelegate>>());
    }

    [Fact]
    public void UseDevelopmentAuth_ReturnsApp_When_Environment_Is_Production()
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
    public async Task UseDevelopmentAuth_Allows_Excluded_Paths_Without_Authentication(string path)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.Method = "GET";

        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        await InvokeMiddlewareAsync(context, next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode); // Default status since next ran
    }

    [Theory]
    [InlineData("AlwaysOn")]
    [InlineData("SiteWarmup")]
    public async Task UseDevelopmentAuth_Allows_Azure_Probes_Without_Authentication(string userAgent)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = "/";
        context.Request.Method = "GET";
        context.Request.Headers.UserAgent = userAgent;

        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        await InvokeMiddlewareAsync(context, next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/home")]
    [InlineData("/assets")] // Not ending with trailing slash, not considered asset folder path
    [InlineData("/robots")] // Not ending with .txt
    [InlineData("/robots.png")]
    public async Task UseDevelopmentAuth_Blocks_Non_Excluded_Paths_Without_Authentication(string path)
    {
        _config["DevelopmentBasicAuthPassword"].Returns("password");
        _env.EnvironmentName.Returns(Environments.Development);

        _app.UseDevelopmentAuth();

        var context = new DefaultHttpContext();
        context.Request.Path = path;
        context.Request.Method = "GET";

        var nextCalled = false;
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        await InvokeMiddlewareAsync(context, next);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
        Assert.Equal("Basic realm=\"Development\"", context.Response.Headers.WWWAuthenticate.ToString());
    }

    [Fact]
    public async Task UseDevelopmentAuth_Allows_Correct_Credentials()
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
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        await InvokeMiddlewareAsync(context, next);

        Assert.True(nextCalled);
        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task UseDevelopmentAuth_Blocks_Incorrect_Credentials()
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
        RequestDelegate next = (ctx) =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };

        await InvokeMiddlewareAsync(context, next);

        Assert.False(nextCalled);
        Assert.Equal(StatusCodes.Status401Unauthorized, context.Response.StatusCode);
    }
}
