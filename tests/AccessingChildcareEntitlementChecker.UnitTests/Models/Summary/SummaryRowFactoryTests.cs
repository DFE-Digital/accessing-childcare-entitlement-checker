using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
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

        // Setup for non-attribute based localisation
        var stringLocalizerFactory = Substitute.For<IStringLocalizerFactory>();
        var localizer = Substitute.For<IStringLocalizer>();
        localizer[Arg.Any<string>()]
            .Returns(callInfo =>
            {
                var key = callInfo.Arg<string>();
                return new LocalizedString(key, key);
            });

        stringLocalizerFactory
            .Create("Views.Home.Location", Arg.Any<string>())
            .Returns(localizer);

        stringLocalizerFactory
            .Create("Views.User.UserAge", Arg.Any<string>())
            .Returns(localizer);

        stringLocalizerFactory
            .Create("Views.User.HasPartner", Arg.Any<string>())
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

        var row = Assert.Single(rows);
        Assert.Equal("Test Enum Property Title", row.Key);
        Assert.Equal("Value One", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
        Assert.False(row.IsLocalised);
    }

    [Theory]
    [InlineData(new TestEnum[] { TestEnum.One }, "Value One")]
    [InlineData(new TestEnum[] { TestEnum.One, TestEnum.Two }, "Value One, Value Two")]
    public void ItExtractsTheDisplayNamesForListOfEnums(IEnumerable<TestEnum> enumList, string expectedValue)
    {
        _summaryRowFactory.Add<TestViewModel, List<TestEnum>, TestEnum>(
            m => m.TestPropertyList,
            enumList.ToList(),
            "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Test List<Enum> Property Title", row.Key);
        Assert.Equal(expectedValue, row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Fact]
    public void ItExtractsTheDisplayNamesForDateOnlies()
    {
        _summaryRowFactory.Add<TestViewModel, DateOnly>(m => m.TestPropertyDateOnly, DateOnly.MinValue, "test-action-name");
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Test DateOnly Property Title", row.Key);
        Assert.Equal("1 January 0001", row.Value);
        Assert.Equal("test-action-name", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Theory]
    [InlineData(CountryOfResidence.Wales, "Option_Wales")]
    [InlineData(CountryOfResidence.England, "Option_England")]
    [InlineData(CountryOfResidence.Scotland, "Option_Scotland")]
    [InlineData(CountryOfResidence.NorthernIreland, "Option_NorthernIreland")]
    public void ItExtractsTheViewResourcesForLocation(CountryOfResidence countryOfResidence, string rowValue)
    {
        _summaryRowFactory.AddLocation(countryOfResidence);
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Title", row.Key);
        Assert.Equal(rowValue, row.Value);
        Assert.Equal("Location", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Theory]
    [InlineData(AgeRange.UnderEighteen, "Option_Under18")]
    [InlineData(AgeRange.EighteenToTwenty, "Option_18To20")]
    [InlineData(AgeRange.TwentyOneOrOver, "Option_21OrOver")]
    public void ItExtractsTheViewResourcesForUserAge(AgeRange ageRange, string rowValue)
    {
        _summaryRowFactory.AddUserAge(ageRange);
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Title", row.Key);
        Assert.Equal(rowValue, row.Value);
        Assert.Equal("UserAge", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void ItExtractsTheViewResourcesForHasPartner(bool hasPartner, string rowValue)
    {
        _summaryRowFactory.AddHasPartner(hasPartner);
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Title", row.Key);
        Assert.Equal(rowValue, row.Value);
        Assert.Equal("HasPartner", row.ChangeAction);
        Assert.Equal("Test", row.ChangeController);
    }

    [Theory]
    [InlineData(AgeRange.UnderEighteen, "Option_Under18")]
    [InlineData(AgeRange.EighteenToTwenty, "Option_18To20")]
    [InlineData(AgeRange.TwentyOneOrOver, "Option_21OrOver")]
    public void ItExtractsTheViewResourcesForPartnerAge(AgeRange ageRange, string rowValue)
    {
        _summaryRowFactory.AddPartnerAge(ageRange);
        var rows = _summaryRowFactory.ViewModels;

        var row = Assert.Single(rows);
        Assert.Equal("Title", row.Key);
        Assert.Equal(rowValue, row.Value);
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
