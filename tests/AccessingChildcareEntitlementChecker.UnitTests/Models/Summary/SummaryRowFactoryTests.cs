using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AccessingChildcareEntitlementChecker.Web;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using AccessingChildcareEntitlementChecker.RulesEngine.Services;
using AccessingChildcareEntitlementChecker.RulesEngine.Extensions;


namespace AccessingChildcareEntitlementChecker.UnitTests.Models.Summary;

public class SummaryRowFactoryTests
{
    private readonly SummaryRowFactory _summaryRowFactory;

    public SummaryRowFactoryTests()
    {
        var services = new ServiceCollection();
        services
            .AddMvcCore()
            .AddDataAnnotations();

        var metadataProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IModelMetadataProvider>();

        var stringLocalizerFactory = Substitute.For<IStringLocalizerFactory>();
        _summaryRowFactory = new SummaryRowFactory(metadataProvider, "Test", stringLocalizerFactory);
    }

    [Fact]
    public void ItExtractsTheDisplayNames()
    {
        _summaryRowFactory.Add<TestViewModel, TestEnum>(m => m.TestProperty, TestEnum.One, "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test Property Title", row.Key);
        Assert.Equal("Value One", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    public enum TestEnum
    {
        [Display(Name = "Value One")]
        One,

        [Display(Name = "Value Two")]
        Two,
    }

    public class TestViewModel
    {
        [Display(Name = "Test Property Title")]
        public TestEnum TestProperty { get; set; }
    }
}
