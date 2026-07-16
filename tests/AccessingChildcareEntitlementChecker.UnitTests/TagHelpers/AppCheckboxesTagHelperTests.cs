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

public class AppCheckboxesTagHelperTests
{
    private readonly TagHelperContext _tagHelperContext;
    private readonly TagHelperOutput _tagHelperOutput;
    private readonly IModelMetadataProvider _metadataProvider;
    private readonly IComponentGenerator _componentGenerator;
    private readonly ViewContext _viewContext;
    private readonly ChildcareSupportViewModel _childcareSupportViewModel;
    private readonly ModelMetadata _modelMetadata;
    private CheckboxesOptions? _generatedOptions;

    public AppCheckboxesTagHelperTests()
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

#pragma warning disable CA2012 // ValueTask instances should be consumed correctly
        _componentGenerator = Substitute.For<IComponentGenerator>();
        _componentGenerator
            .GenerateCheckboxesAsync(Arg.Do<CheckboxesOptions>(options => _generatedOptions = options))
            .Returns(_ => ValueTask.FromResult<GovUkComponent>(new TestGovUkComponent()));
#pragma warning restore CA2012

        _tagHelperContext = new TagHelperContext(
            tagName: "app-checkboxes",
            allAttributes: [],
            items: new Dictionary<object, object>(),
            uniqueId: "test");

        _tagHelperOutput = new TagHelperOutput(
            "app-checkboxes",
            attributes: [],
            getChildContentAsync: (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        _childcareSupportViewModel = new ChildcareSupportViewModel
        {
            ChildcareSupport = [],
        };

        _modelMetadata = _metadataProvider.GetMetadataForProperty(
            typeof(ChildcareSupportViewModel),
            nameof(ChildcareSupportViewModel.ChildcareSupport));
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_Generates_Default()
    {
        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Fieldset?.Legend?.Text);
        var actual = _generatedOptions.Fieldset.Legend.Text.ToHtmlString();
        Assert.Equal("Do you already get any of these to help pay for childcare?", actual);

        Assert.NotNull(_generatedOptions?.Items);
        Assert.Equal(3, _generatedOptions.Items.Count);

        var first = _generatedOptions.Items.First();
        Assert.NotNull(first?.Value);
        Assert.NotNull(first?.Text);
        Assert.Equal("0", first.Value.ToHtmlString());
        Assert.Equal("Childcare vouchers", first.Text.ToHtmlString());
        Assert.NotNull(first?.Hint?.Text);
        Assert.Equal(
            "A scheme that lets you pay for childcare from your salary before tax, which closed to new applicants in October 2018",
            first.Hint.Text.ToHtmlString());


        var last = _generatedOptions.Items.Last();
        Assert.NotNull(last?.Value);
        Assert.NotNull(last?.Text);
        Assert.Equal("2", last.Value.ToHtmlString());
        Assert.Equal("No, I do not get any of these", last.Text.ToHtmlString());
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_Exclusive_Generates_Exclusive()
    {
        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
        {
            For = modelExpression,
            ExclusiveValue = ChildcareSupportOption.None,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Items);
        Assert.Equal(4, _generatedOptions.Items.Count);

        var separator = _generatedOptions.Items.Skip(2).First();
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
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.NotNull(_generatedOptions?.Hint?.Text);
        var actual = _generatedOptions.Hint.Text.ToHtmlString();
        Assert.Equal("Select all that apply", actual);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_No_Hint_Does_Not_Generate_Hint()
    {
        var fakeViewModel = new FakeViewModel
        {
            ChildcareSupport = []
        };

        var modelMetadata = _metadataProvider.GetMetadataForProperty(
            typeof(FakeViewModel),
            nameof(FakeViewModel.ChildcareSupport));

        var modelExpression = new ModelExpression(
            nameof(FakeViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, modelMetadata, fakeViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
        {
            For = modelExpression,
            ViewContext = _viewContext
        };

        await helper.ProcessAsync(_tagHelperContext, _tagHelperOutput);

        Assert.Null(_generatedOptions?.Hint);
    }

    [Fact]
    public async Task ProcessAsync_With_Enum_Collection_And_Legend_Generates_Legend()
    {
        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
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
        _viewContext.ModelState.AddModelError(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            "ERROR");

        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
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
        _childcareSupportViewModel.ChildcareSupport = [ChildcareSupportOption.ChildcareVouchers];

        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
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
        _viewContext.ModelState.SetModelValue("ChildcareSupport", "0", "0");
        var modelExpression = new ModelExpression(
            nameof(ChildcareSupportViewModel.ChildcareSupport),
            new ModelExplorer(_metadataProvider, _modelMetadata, _childcareSupportViewModel.ChildcareSupport));

        var helper = new AppCheckboxesTagHelper(_componentGenerator)
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

    public class FakeViewModel
    {
        public List<ChildcareSupportOption> ChildcareSupport { get; set; } = new();
    }
}