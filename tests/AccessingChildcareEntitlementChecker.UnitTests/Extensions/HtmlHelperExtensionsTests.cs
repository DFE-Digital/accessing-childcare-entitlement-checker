using AccessingChildcareEntitlementChecker.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Extensions;

public class HtmlHelperExtensionsTests
{
    private readonly ServiceProvider _serviceProvider;

    public HtmlHelperExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddControllersWithViews();

        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public void GetDescriptionFor_ReturnsDisplayAttributeDescription()
    {
        var serviceProvider = _serviceProvider;
        var metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();
        var htmlHelper = Substitute.For<IHtmlHelper<TestViewModel>>();
        var viewContext = new ViewContext
        {
            HttpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider,
            },
        };

        var viewData = new ViewDataDictionary<TestViewModel>(
            metadataProvider,
            new ModelStateDictionary())
        {
            Model = new TestViewModel(),
        };

        htmlHelper.ViewContext.Returns(viewContext);
        htmlHelper.ViewData.Returns(viewData);

        var description = htmlHelper.GetDescriptionFor(model => model.PropertyWithDescription);

        Assert.Equal("The display attribute description", description);
    }

    public class TestViewModel
    {
        [Display(Description = "The display attribute description")]
        public string? PropertyWithDescription { get; set; }
    }
}
