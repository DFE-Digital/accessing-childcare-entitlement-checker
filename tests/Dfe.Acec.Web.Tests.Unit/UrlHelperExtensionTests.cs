using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit;

public class UrlHelperExtensionTests
{
    private readonly IUrlHelper _urlHelper;

    public UrlHelperExtensionTests()
    {
        _urlHelper = Substitute.For<IUrlHelper>();

        _urlHelper
            .Action(Arg.Any<UrlActionContext>())
            .Returns((string?)null);
    }

    [Fact]
    public void WhenUrlDoesNotExistItWillThrow()
    {
        Assert.Throws<InvalidOperationException>(() => _urlHelper.ActionOrThrow("NonExistentRoute"));
    }

    [Fact]
    public void WhenUrlDoesNotExistWithControllerItWillThrow()
    {
        Assert.Throws<InvalidOperationException>(() => _urlHelper.ActionOrThrow("NonExistentRoute", "NonExistentController"));
    }
}
