using AccessingChildcareEntitlementChecker.Web.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AccessingChildcareEntitlementChecker.UnitTests.TagHelpers;

public class NonceTagHelperTests
{
    [Fact]
    public void Process_WithAddNonceStringTrue_RemovesAddNonceAndAddsCspNonce()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        const string expectedNonce = "test-csp-nonce-value-12345";
        httpContext.Items["csp-nonce"] = expectedNonce;

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", "true");

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain(addNonceAttribute, output.Attributes);
        var nonceAttribute = Assert.Single(output.Attributes);
        Assert.Equal("nonce", nonceAttribute.Name);
        Assert.Equal(expectedNonce, nonceAttribute.Value);
    }

    [Fact]
    public void Process_WithAddNonceBoolTrue_RemovesAddNonceAndAddsCspNonce()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        const string expectedNonce = "boolean-true-csp-nonce-value";
        httpContext.Items["csp-nonce"] = expectedNonce;

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", true);

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain(addNonceAttribute, output.Attributes);
        var nonceAttribute = Assert.Single(output.Attributes);
        Assert.Equal("nonce", nonceAttribute.Name);
        Assert.Equal(expectedNonce, nonceAttribute.Value);
    }

    [Fact]
    public void Process_WithAddNonceStringFalse_DoesNotRemoveAddNonceAndDoesNotAddCspNonce()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            Items =
            {
                ["csp-nonce"] = "some-nonce"
            }
        };

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", "false");

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains(addNonceAttribute, output.Attributes);
        Assert.DoesNotContain(output.Attributes, attr => attr.Name == "nonce");
    }

    [Fact]
    public void Process_WithAddNonceBoolFalse_DoesNotRemoveAddNonceAndDoesNotAddCspNonce()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            Items =
            {
                ["csp-nonce"] = "some-nonce"
            }
        };

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", false);

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains(addNonceAttribute, output.Attributes);
        Assert.DoesNotContain(output.Attributes, attr => attr.Name == "nonce");
    }

    [Fact]
    public void Process_WithAddNonceInvalid_DoesNotRemoveAddNonceAndDoesNotAddCspNonce()
    {
        // Arrange
        var httpContext = new DefaultHttpContext
        {
            Items =
            {
                ["csp-nonce"] = "some-nonce"
            }
        };

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", "not-a-boolean");

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Contains(addNonceAttribute, output.Attributes);
        Assert.DoesNotContain(output.Attributes, attr => attr.Name == "nonce");
    }

    [Fact]
    public void Process_WithAddNonceTrueAndNullViewContext_RemovesAddNonceAndAddsNonceWithNullValue()
    {
        // Arrange
        var tagHelper = new NonceTagHelper
        {
            ViewContext = null
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", "true");

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain(addNonceAttribute, output.Attributes);
        var nonceAttribute = Assert.Single(output.Attributes);
        Assert.Equal("nonce", nonceAttribute.Name);
        Assert.Null(nonceAttribute.Value);
    }

    [Fact]
    public void Process_WithAddNonceTrueAndMissingCspNonce_RemovesAddNonceAndAddsNonceWithNullValue()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        // Do not add "csp-nonce" to HttpContext.Items

        var viewContext = new ViewContext
        {
            HttpContext = httpContext
        };

        var tagHelper = new NonceTagHelper
        {
            ViewContext = viewContext
        };

        var addNonceAttribute = new TagHelperAttribute("add-nonce", "true");

        var context = new TagHelperContext(
            [addNonceAttribute],
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var output = new TagHelperOutput(
            "script",
            [addNonceAttribute],
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.DoesNotContain(addNonceAttribute, output.Attributes);
        var nonceAttribute = Assert.Single(output.Attributes);
        Assert.Equal("nonce", nonceAttribute.Name);
        Assert.Null(nonceAttribute.Value);
    }
}
