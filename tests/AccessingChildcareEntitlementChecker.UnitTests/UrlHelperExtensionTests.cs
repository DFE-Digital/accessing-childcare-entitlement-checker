using AccessingChildcareEntitlementChecker.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests;

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
    public void When_Url_Does_Not_Exist_It_Will_Throw()
    {
        Assert.Throws<InvalidOperationException>(() => _urlHelper.ActionOrThrow("NonExistentRoute"));
    }

    [Fact]
    public void When_Url_Does_Not_Exist_With_Controller_It_Will_Throw()
    {
        Assert.Throws<InvalidOperationException>(() => _urlHelper.ActionOrThrow("NonExistentRoute", "NonExistentController"));
    }
}
