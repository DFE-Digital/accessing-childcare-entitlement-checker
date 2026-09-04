using AccessingChildcareEntitlementChecker.Web.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Extensions;

public class HtmlLocalizerExtensionsTests
{
    private readonly IHtmlLocalizer _localizer;

    public HtmlLocalizerExtensionsTests()
    {
        _localizer = Substitute.For<IHtmlLocalizer>();
    }

    [Fact]
    public void MaskChildName_WrapsNameInClarityMask_WhenNameIsProvided()
    {
        // Arrange
        const string key = "Heading";
        const string childName = "John";
        _localizer[key, Arg.Any<object[]>()].Returns(x =>
            new LocalizedHtmlString((string)x[0], "Hello {0}"));

        // Act
        var result = _localizer.MaskChildName(key, childName);

        // Assert
        Assert.NotNull(result);
        _ = _localizer.Received(1)[key, Arg.Is<object[]>(args =>
            args.Length == 1 &&
            args[0] is HtmlString &&
            ((HtmlString)args[0]).Value == "<span data-clarity-mask=\"true\">John</span>"
        )];
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void MaskChildName_UsesFallback_WhenNameIsNullOrEmptyOrWhitespace(string? invalidName)
    {
        // Arrange
        const string key = "Heading";
        _localizer[key, Arg.Any<object[]>()].Returns(x =>
            new LocalizedHtmlString((string)x[0], "Hello {0}"));

        // Act
        var result = _localizer.MaskChildName(key, invalidName);

        // Assert
        Assert.NotNull(result);
        _ = _localizer.Received(1)[key, Arg.Is<object[]>(args =>
            args.Length == 1 &&
            args[0] is HtmlString &&
            ((HtmlString)args[0]).Value == "<span data-clarity-mask=\"true\">child</span>"
        )];
    }

    [Fact]
    public void MaskChildName_EncodesName_ToPreventXss()
    {
        // Arrange
        const string key = "Heading";
        const string maliciousName = "<script>alert('xss')</script>";
        _localizer[key, Arg.Any<object[]>()].Returns(x =>
            new LocalizedHtmlString((string)x[0], "Hello {0}"));

        // Act
        var result = _localizer.MaskChildName(key, maliciousName);

        // Assert
        Assert.NotNull(result);
        _ = _localizer.Received(1)[key, Arg.Is<object[]>(args =>
            args.Length == 1 &&
            args[0] is HtmlString &&
            ((HtmlString)args[0]).Value == "<span data-clarity-mask=\"true\">&lt;script&gt;alert(&#x27;xss&#x27;)&lt;/script&gt;</span>"
        )];
    }

    [Fact]
    public void MaskChildName_AppendsAdditionalArgumentsCorrectly()
    {
        // Arrange
        const string key = "ComplexHeading";
        const string childName = "Jane";
        const string additionalArg1 = "first";
        const int additionalArg2 = 42;
        _localizer[key, Arg.Any<object[]>()].Returns(x =>
            new LocalizedHtmlString((string)x[0], "Hello {0}, {1}, {2}"));

        // Act
        var result = _localizer.MaskChildName(key, childName, additionalArg1, additionalArg2);

        // Assert
        Assert.NotNull(result);
        _ = _localizer.Received(1)[key, Arg.Is<object[]>(args =>
            args.Length == 3 &&
            args[0] is HtmlString &&
            ((HtmlString)args[0]).Value == "<span data-clarity-mask=\"true\">Jane</span>" &&
            args[1] is string && (string)args[1] == "first" &&
            args[2] is int && (int)args[2] == 42
        )];
    }
}
