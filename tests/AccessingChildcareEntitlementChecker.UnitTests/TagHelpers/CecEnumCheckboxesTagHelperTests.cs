using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.TagHelpers;
using GovUk.Frontend.AspNetCore;
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
    private readonly IModelMetadataProvider _metadataProvider;
    private readonly IComponentGenerator _componentGenerator;
    private readonly ViewContext _viewContext;
    private readonly BenefitsViewModel _benefitsViewModel;
    private readonly ModelMetadata _modelMetadata;
    private CheckboxesOptions? _generatedOptions;

    public CecEnumCheckboxesTagHelperTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddMvcCore().AddDataAnnotations();
        using var serviceProvider = services.BuildServiceProvider();
        _metadataProvider = serviceProvider.GetRequiredService<IModelMetadataProvider>();

        var httpContext = new DefaultHttpContext();
        _viewContext = new ViewContext
        {
            HttpContext = httpContext,
            ViewData = new ViewDataDictionary(_metadataProvider, new ModelStateDictionary())
        };

        _componentGenerator = Substitute.For<IComponentGenerator>();
        _componentGenerator
            .GenerateCheckboxesAsync(Arg.Do<CheckboxesOptions>(options => _generatedOptions = options))
            .Returns(callInfo => ValueTask.FromResult<GovUkComponent>(new TestGovUkComponent()));

        _tagHelperContext = new TagHelperContext(
            tagName: "cec-enum-checkboxes",
            allAttributes: [],
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        _tagHelperOutput = new TagHelperOutput(
            "cec-enum-checkboxes",
            attributes: [],
            getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        _benefitsViewModel = new BenefitsViewModel
        {
            Benefits = [],
        };

        _modelMetadata = _metadataProvider.GetMetadataForProperty(typeof(BenefitsViewModel), nameof(BenefitsViewModel.Benefits));
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_Generates_Default()
    {
        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Fieldset?.Legend?.Text);
        var actual = _generatedOptions.Fieldset.Legend.Text.ToHtmlString();
        Assert.Equal("Do you get any of these benefits?", actual);

        Assert.NotNull(_generatedOptions?.Items);
        Assert.Equal(9, _generatedOptions.Items.Count);

        var first = _generatedOptions.Items.First();
        Assert.NotNull(first?.Value);
        Assert.NotNull(first?.Text);
        Assert.Equal("0", first.Value.ToHtmlString());
        Assert.Equal("Carer&#x27;s Allowance", first.Text.ToHtmlString());

        var last = _generatedOptions.Items.Last();
        Assert.NotNull(last?.Value);
        Assert.NotNull(last?.Text);
        Assert.Equal("8", last.Value.ToHtmlString());
        Assert.Equal("No, I do not get any of these benefits", last.Text.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_Exclusive_Generates_Exclusive()
    {
        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            ExclusiveValue = BenefitsOption.None,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Items);
        Assert.Equal(10, _generatedOptions.Items.Count);

        var separator = _generatedOptions.Items.Skip(8).First();
        Assert.NotNull(separator?.Divider);
        Assert.Equal("or", separator.Divider.ToHtmlString());

        var last = _generatedOptions.Items.Last();
        Assert.NotNull(last?.Behaviour);
        Assert.Equal("exclusive", last.Behaviour.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_Hint_Generates_Hint()
    {
        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            Hint = "Select all that apply",
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Hint?.Text);
        var actual = _generatedOptions.Hint.Text.ToHtmlString();
        Assert.Equal("Select all that apply", actual);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_Legend_Generates_Legend()
    {
        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            Legend = new HtmlString("Custom Legend"),
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Fieldset?.Legend?.Html);
        var actual = _generatedOptions.Fieldset.Legend.Html.ToHtmlString();
        Assert.Equal("Custom Legend", actual);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_Adds_Error()
    {
        _viewContext.ModelState.AddModelError(nameof(BenefitsViewModel.Benefits), "ERROR");

        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.ErrorMessage?.Text);
        var actual = _generatedOptions.ErrorMessage.Text.ToHtmlString();
        Assert.Equal("ERROR", actual);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_Selects_Checkboxes_From_Model()
    {
        _benefitsViewModel.Benefits = [BenefitsOption.CarersAllowance];

        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Items);
        var first = _generatedOptions.Items.First();
        Assert.True(first?.Checked);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_Selects_Checkboxes_From_ModelState()
    {
        _viewContext.ModelState.SetModelValue("Benefits", "0", "0");
        var modelExpression = new ModelExpression(
            nameof(BenefitsViewModel.Benefits),
            new ModelExplorer(_metadataProvider, _modelMetadata, _benefitsViewModel.Benefits));

        var helper = new CecEnumCheckboxesTagHelper(_componentGenerator, _metadataProvider)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Items);
        var first = _generatedOptions.Items.First();
        Assert.True(first?.Checked);
    }

    public class TestGovUkComponent : GovUkComponent
    {
        public override IHtmlContent GetContent() => HtmlString.Empty;

        public override void ApplyToTagHelper(TagHelperOutput output) => output.SuppressOutput();
    }
}