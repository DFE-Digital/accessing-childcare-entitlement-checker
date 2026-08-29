using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Services.Summary;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public static class SummaryControllerTestFactory
{
    public static SummaryController Create(
        JourneyState journeyState,
        IJourneySession journeySession,
        IValidator<JourneyState> validator,
        FakeLogger<SummaryController> logger,
        ISummaryViewModelBuilder summaryViewModelBuilder)
    {
        var controller = new SummaryController(
            journeyState,
            journeySession,
            validator,
            logger,
            summaryViewModelBuilder);

        var services = new ServiceCollection();
        services
            .AddMvcCore()
            .AddDataAnnotations();

        var metadataProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IModelMetadataProvider>();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        controller.TempData = new TempDataDictionary(controller.HttpContext, Substitute.For<ITempDataProvider>());
        controller.MetadataProvider = metadataProvider;
        controller.Url = Substitute.For<IUrlHelper>();
        controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");

        return controller;
    }

    public static ISummaryViewModelBuilder CreateRealViewModelBuilder(IFeatureManager featureManager)
    {
        var services = new ServiceCollection();
        services
            .AddMvcCore()
            .AddDataAnnotations();

        var metadataProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IModelMetadataProvider>();

        var stringLocalizerFactory = AcecSubstitute.ForLocalizerFactory();

        var userSummaryBuilder = new UserSummaryBuilder(metadataProvider, stringLocalizerFactory, featureManager);
        var partnerSummaryBuilder = new PartnerSummaryBuilder(metadataProvider, stringLocalizerFactory);
        var childSummaryBuilder = new ChildSummaryBuilder(metadataProvider, stringLocalizerFactory);
        return new SummaryViewModelBuilder(userSummaryBuilder, partnerSummaryBuilder, childSummaryBuilder);
    }

    public static JourneyState CreateDefaultJourneyState(string childId)
    {
        return new JourneyState
        {
            Nationality = NationalityOption.BritishOrIrishCitizen,
            Children =
            {
                [childId] = new Child(childId, "Child A")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = new DateOnly(2020, 1, 1),
                    ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
                }
            }
        };
    }
}
