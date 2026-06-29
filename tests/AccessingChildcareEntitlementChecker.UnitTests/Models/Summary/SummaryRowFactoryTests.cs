using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
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

    [Fact]
    public void ParentalLeave_Returns_No_Summary_When_Not_Answered()
    {
        var journeyState = new JourneyState();
        var value = new List<string>();
        _summaryRowFactory.AddParentalLeave(journeyState);

        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void ParentalLeave_Returns_No_Summary_When_None_Selected()
    {
        var journeyState = new JourneyState();
        var value = new List<string>() { ParentalLeaveViewModel.NoneSelectedValue };
        _summaryRowFactory.AddParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void ParentalLeave_Returns_No_Summary_When_Child_Does_Not_Exist()
    {
        var journeyState = new JourneyState();
        var value = new List<string>() { "1" };
        _summaryRowFactory.AddParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void ParentalLeave_Returns_Child_Name_When_Selected()
    {
        var journeyState = new JourneyState
        {
            Children = new Dictionary<string, Child>
            {
                { "1", new Child("1", "Child One") },
                { "2", new Child("2", "Child Two") }
            },
            ParentalLeaveChildrenIds = ["1", "2"],
        };

        _summaryRowFactory.AddParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Single(rows);
        Assert.Equal("Which child are you on leave for?", rows[0].Key);
        Assert.Equal("Child One, Child Two", rows[0].Value);
        Assert.Equal("ParentalLeave", rows[0].ChangeAction);
        Assert.Equal("Test", rows[0].ChangeController);
    }

    [Fact]
    public void PartnerParentalLeave_Returns_No_Summary_When_Not_Answered()
    {
        var journeyState = new JourneyState();
        var value = new List<string>();
        _summaryRowFactory.AddPartnerParentalLeave(journeyState);

        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void PartnerParentalLeave_Returns_No_Summary_When_None_Selected()
    {
        var journeyState = new JourneyState();
        var value = new List<string>() { PartnerParentalLeaveViewModel.NoneSelectedValue };
        _summaryRowFactory.AddPartnerParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void PartnerParentalLeave_Returns_No_Summary_When_Child_Does_Not_Exist()
    {
        var journeyState = new JourneyState();
        var value = new List<string>() { "1" };
        _summaryRowFactory.AddPartnerParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Empty(rows);
    }

    [Fact]
    public void PartnerParentalLeave_Returns_Child_Name_When_Selected()
    {
        var journeyState = new JourneyState
        {
            Children = new Dictionary<string, Child>
            {
                { "1", new Child("1", "Child One") },
                { "2", new Child("2", "Child Two") }
            },
            PartnerParentalLeaveChildrenIds = ["1", "2"],
        };

        _summaryRowFactory.AddPartnerParentalLeave(journeyState);
        var rows = _summaryRowFactory.ViewModels;
        Assert.Single(rows);
        Assert.Equal("Which child is your partner on leave for?", rows[0].Key);
        Assert.Equal("Child One, Child Two", rows[0].Value);
        Assert.Equal("PartnerParentalLeave", rows[0].ChangeAction);
        Assert.Equal("Test", rows[0].ChangeController);
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
