using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class JourneyTests
{
    private const string ChildId = "child-a";

    [Fact]
    public void Backwards_FromCheckChildDetails_ForBornChild_ReturnsChildSupport()
    {
        var journey = CreateJourney();
        var journeyState = CreateJourneyState(BirthStatus.Born);
        var controller = CreateController(nameof(SummaryController), nameof(SummaryController.CheckChildDetails));

        var result = journey.Backwards(controller, journeyState, new { childId = ChildId });

        Assert.Equal("/BornChildDetails/ChildSupport?childId=child-a", result.Value);
    }

    [Fact]
    public void Backwards_FromCheckChildDetails_ForExpectedChild_ReturnsExpectedChildRelationship()
    {
        var journey = CreateJourney();
        var journeyState = CreateJourneyState(BirthStatus.Due);
        var controller = CreateController(nameof(SummaryController), nameof(SummaryController.CheckChildDetails));

        var result = journey.Backwards(controller, journeyState, new { childId = ChildId });

        Assert.Equal("/ExpectedChildDetails/ExpectedChildRelationship?childId=child-a", result.Value);
    }

    private static Journey CreateJourney()
    {
        var builder = new JourneyBuilder();
        var home = builder.Add<HomeController>("/start-page", nameof(HomeController.Start));
        var childName = builder.Add<IntroductionController>("/children/{childId}/add-child-details", nameof(IntroductionController.ChildName));
        var isChildBorn = builder.Add<IntroductionController>("/children/{childId}/has-the-child-been-born", nameof(IntroductionController.IsChildBorn));
        var childBirthDate = builder.Add<BornChildDetailsController>("/children/{childId}/childs-date-of-birth", nameof(BornChildDetailsController.ChildBirthDate));
        var childRelationship = builder.Add<BornChildDetailsController>("/children/{childId}/relationship", nameof(BornChildDetailsController.ChildRelationship));
        var childSupport = builder.Add<BornChildDetailsController>("/children/{childId}/child-support", nameof(BornChildDetailsController.ChildSupport));
        var childDueDate = builder.Add<ExpectedChildDetailsController>("/children/{childId}/expectant-childs-due-date", nameof(ExpectedChildDetailsController.ChildDueDate));
        var expectedChildRelationship = builder.Add<ExpectedChildDetailsController>("/children/{childId}/relationship", nameof(ExpectedChildDetailsController.ExpectedChildRelationship));
        var checkChildren = builder.Add<SummaryController>("/children/check-childs-details", nameof(SummaryController.CheckChildDetails));

        home
            .Then(childName)
            .Then(isChildBorn);

        isChildBorn
            .When(s => s.Children, c => c.BirthStatus == BirthStatus.Born, childBirthDate)
            .Then(childDueDate);

        childBirthDate
            .Then(childRelationship)
            .Then(childSupport)
            .Then(checkChildren);

        childDueDate
            .Then(expectedChildRelationship)
            .Then(checkChildren);

        return builder.Build(home);
    }

    private static JourneyState CreateJourneyState(BirthStatus birthStatus)
    {
        return new JourneyState
        {
            Children =
            {
                [ChildId] = new Child(ChildId, "Child A")
                {
                    BirthStatus = birthStatus
                }
            }
        };
    }

    private static TestController CreateController(string controllerName, string actionName)
    {
        var urlHelper = Substitute.For<IUrlHelper>();
        urlHelper.Action(Arg.Any<UrlActionContext>()).Returns(callInfo =>
        {
            var context = callInfo.Arg<UrlActionContext>();
            var childId = context.Values is RouteValueDictionary values
                ? values["childId"]
                : null;

            return $"/{context.Controller}/{context.Action}?childId={childId}";
        });

        return new TestController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext(),
                ActionDescriptor = new ControllerActionDescriptor
                {
                    ControllerName = controllerName.Replace("Controller", string.Empty),
                    ActionName = actionName
                }
            },
            Url = urlHelper
        };
    }

    private sealed class TestController : Controller
    {
    }
}
