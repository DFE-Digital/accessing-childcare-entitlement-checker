using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.TagHelpers;
using GovUk.Frontend.AspNetCore.ComponentGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.TagHelpers;

public class CecEnumCheckboxesTagHelperTests
{
    private readonly TagHelperContext _tagHelperContext;
    private readonly TagHelperOutput _tagHelperOutput;
    private CheckboxesOptions? _generatedOptions;
    private CecEnumCheckboxesTagHelper _helper;


    public CecEnumCheckboxesTagHelperTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMvcCore().AddDataAnnotations();
        using var serviceProvider = services.BuildServiceProvider();
        var metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();

        var model = new BenefitsViewModel
        {
            Benefits = [BenefitsOption.CarersAllowance]
        };

        var metadata = metadataProvider.GetMetadataForProperty(typeof(BenefitsViewModel), nameof(BenefitsViewModel.Benefits));
        var modelExpression = new ModelExpression(nameof(BenefitsViewModel.Benefits), new ModelExplorer(metadataProvider, metadata, model.Benefits));

        //const string errorMessage = "Select any benefits you get";

        var httpContext = new DefaultHttpContext();
        var viewContext = new ViewContext
        {
            HttpContext = httpContext,
            ViewData = new ViewDataDictionary(metadataProvider, new ModelStateDictionary())
        };
        viewContext.ModelState.SetModelValue(
            nameof(BenefitsViewModel.Benefits),
            BenefitsOption.CarersAllowance.ToString(),
            BenefitsOption.CarersAllowance.ToString());
        //viewContext.ModelState.AddModelError(nameof(BenefitsViewModel.Benefits), errorMessage);

        var componentGenerator = Substitute.For<IComponentGenerator>();
        componentGenerator
            .GenerateCheckboxesAsync(Arg.Do<CheckboxesOptions>(options => _generatedOptions = options))
            .Returns(ValueTask.FromResult<GovUkComponent>(new TestGovUkComponent()));

        _helper = new CecEnumCheckboxesTagHelper(componentGenerator, metadataProvider)
        {
            For = modelExpression,
            ExclusiveValue = BenefitsOption.None,
            Hint = "Select all that apply",
            Legend = new HtmlString("Which benefits does <strong>Alex</strong> get?"),
            ViewContext = viewContext
        };
        
        _tagHelperContext = new TagHelperContext(
            tagName: "cec-enum-checkboxes",
            allAttributes: [],
            items: new Dictionary<object, object>(),
            uniqueId: "test");
        _tagHelperOutput = new TagHelperOutput(
            "cec-enum-checkboxes",
            attributes: [],
            getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));
    }

    [Fact]
    public async Task ProcessAsync_WithEnumCollection_GeneratesExpectedCheckboxes()
    {
        // Act
        await _helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        // Assert
        Assert.NotNull(_generatedOptions);
        Assert.Equal(nameof(BenefitsViewModel.Benefits), _generatedOptions.Name);    
        /*
        Assert.Equal(nameof(BenefitsViewModel.Benefits), generatedOptions.Name);
        Assert.Equal(nameof(BenefitsViewModel.Benefits), generatedOptions.IdPrefix);
        Assert.Equal("Select all that apply", generatedOptions.Hint?.Text);
        Assert.Equal(errorMessage, generatedOptions.ErrorMessage?.Text);
        Assert.Equal("Which benefits does <strong>Alex</strong> get?", generatedOptions.Fieldset?.Legend?.Html);
        Assert.Equal(TemplateString.Empty, generatedOptions.Fieldset?.Legend?.Text);
        Assert.True(generatedOptions.Fieldset?.Legend?.IsPageHeading);
        Assert.Equal("govuk-fieldset__legend--l", generatedOptions.Fieldset?.Legend?.Classes);

        var items = Assert.IsAssignableFrom<IReadOnlyCollection<CheckboxesOptionsItem?>>(generatedOptions.Items);
        Assert.Equal(Enum.GetValues<BenefitsOption>().Length + 1, items.Count);

        var firstItem = Assert.IsType<CheckboxesOptionsItem>(items.First());
        Assert.Equal(BenefitsOption.CarersAllowance.ToString(), firstItem.Value);
        Assert.Equal("Carer's Allowance", firstItem.Text);
        Assert.True(firstItem.Checked);

        var divider = Assert.IsType<CheckboxesOptionsItem>(items.ElementAt(items.Count - 2));
        Assert.Equal("or", divider.Divider);

        var exclusiveItem = Assert.IsType<CheckboxesOptionsItem>(items.Last());
        Assert.Equal(BenefitsOption.None.ToString(), exclusiveItem.Value);
        Assert.Equal("exclusive", exclusiveItem.Behaviour);
        Assert.Single(httpContext.Items);*/
    }

    private sealed class TestGovUkComponent : GovUkComponent
    {
        public override IHtmlContent GetContent() => HtmlString.Empty;

        public override void ApplyToTagHelper(TagHelperOutput output) => output.SuppressOutput();
    }
}
