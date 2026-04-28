using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AccessingChildcareEntitlementChecker.Web.Controllers
{
    public class JourneyActions
    {
        private readonly IJourneySession _journeySession;
        private readonly JourneyState _journeyState;
        private readonly Controller _controller;

        private JourneyActions(IJourneySession journeySession, JourneyState journeyState, Controller controller)
        {
            _journeySession = journeySession;
            _journeyState = journeyState;
            _controller = controller;
        }

        public IActionResult HandlePost<T, TController>(T model, Action<JourneyState> apply, Expression<Action<TController>> nextAction)
            where TController : Controller
        {
            if (!_controller.ModelState.IsValid)
            {
                return _controller.View(model);
            }

            apply(_journeyState);
            _journeySession.Set(_journeyState);
            
            var body = (MethodCallExpression) nextAction.Body;
            var actionName = body.Method.Name;
            var controllerType = typeof(TController);
            var controllerName = controllerType.Name;
            var routeControllerName = controllerName[..^"Controller".Length];
            return _controller.RedirectToAction(actionName, routeControllerName);
        }

        public class Factory
        {
            private readonly IJourneySession _journeySession;
            private readonly JourneyState _journeyState;

            public Factory(IJourneySession journeySession, JourneyState journeyState)
            {
                _journeySession = journeySession;
                _journeyState = journeyState;
            }

            public JourneyActions Create(Controller controller)
            {
                return new JourneyActions(_journeySession, _journeyState, controller);
            }
        }
    }
}
