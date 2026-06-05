using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NSubstitute.Core.Arguments;
using System.ComponentModel.DataAnnotations;


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

        var localizer = Substitute.For<IStringLocalizer>();
        localizer["Title"].Returns(new LocalizedString("Title", "Test Title"));
        localizer["Option_Wales"].Returns(new LocalizedString("Option_Wales", "Test Country"));
        localizer["Option_Under18"].Returns(new LocalizedString("Option_Under18", "Test Age"));

        stringLocalizerFactory
            .Create("Views.Home.Location", Arg.Any<string>())
            .Returns(localizer);

        stringLocalizerFactory
            .Create("Views.Partner.PartnerAge", Arg.Any<string>())
            .Returns(localizer);

        _summaryRowFactory = new SummaryRowFactory(metadataProvider, "Test", stringLocalizerFactory);
    }

    [Fact]
    public void ItExtractsTheDisplayNamesForEnums()
    {
        _summaryRowFactory.Add<TestViewModel, TestEnum>(m => m.TestProperty, TestEnum.One, "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test Enum Property Title", row.Key);
        Assert.Equal("Value One", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
        Assert.False(row.IsLocalised);
    }

    [Fact]
    public void ItExtractsTheDisplayNamesForListOfEnums()
    {
        _summaryRowFactory.Add<TestViewModel, List<TestEnum>, TestEnum>(
            m => m.TestPropertyList,
            [TestEnum.One, TestEnum.Two],
            "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test List<Enum> Property Title", row.Key);
        Assert.Equal("Value One, Value Two", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Fact]
    public void ItExtractsTheDisplayNamesForDateOnlies()
    {
        _summaryRowFactory.Add<TestViewModel, DateOnly>(m => m.TestPropertyDateOnly, DateOnly.MinValue, "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test DateOnly Property Title", row.Key);
        Assert.Equal("1 January 0001", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Fact]
    public void ItExtractsTheViewResourcesForLocation()
    {
        _summaryRowFactory.AddLocation(CountryOfResidence.Wales);
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test Title", row.Key);
        Assert.Equal("Test Country", row.Value);
        Assert.Equal("Location", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Fact]
    public void ItExtractsTheViewResourcesForPartnerAge()
    {
        _summaryRowFactory.AddPartnerAge(AgeRange.UnderEighteen);
        var rows = _summaryRowFactory.ViewModels;

        Assert.Single(rows);
        var row = rows.First();
        Assert.Equal("Test Title", row.Key);
        Assert.Equal("Test Age", row.Value);
        Assert.Equal("PartnerAge", row.ChangeAction);
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
        [Display(Name = "Test Enum Property Title")]
        public TestEnum TestProperty { get; set; }

        [Display(Name = "Test List<Enum> Property Title")]
        public List<TestEnum>? TestPropertyList { get; set; }

        [Display(Name = "Test DateOnly Property Title")]
        public DateOnly TestPropertyDateOnly { get; set; }
    }
}
