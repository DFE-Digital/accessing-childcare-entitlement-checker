using System.Linq.Expressions;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public class JourneyPageBuilder
    {
        private string _url;
        private PageKey _pageKey;
        private List<Edge> _edges;

        public JourneyPageBuilder(string controllerName, string actionName, string url)
        {
            _pageKey = new PageKey(controllerName, actionName);
            _url = url;
            _edges = new List<Edge>();
        }

        public PageKey PageKey => _pageKey;

        public JourneyPageBuilder Then(JourneyPageBuilder nextPage)
        {
            _edges.Add(new Edge(_ => true, nextPage.PageKey));
            return nextPage;
        }

        public JourneyPageBuilder When(Expression<Func<JourneyState, bool>> condition, JourneyPageBuilder nextPage)
        {
            var compiledCondition = condition.Compile();
            _edges.Add(new Edge(context => compiledCondition(context.JourneyState), nextPage.PageKey));
            return this;
        }

        public JourneyPageBuilder When(
            Expression<Func<JourneyState, Dictionary<string, Child>>> collectionSelector,
            Expression<Func<Child, bool>> condition,
            JourneyPageBuilder nextPage)
        {
            var compiledSelector = collectionSelector.Compile();
            var compiledCondition = condition.Compile();
            _edges.Add(new Edge(context =>
            {
                var childId = context.RouteValues["childId"]?.ToString();
                if (childId == null)
                {
                    return false;
                }

                var children = compiledSelector(context.JourneyState);
                if (!children.TryGetValue(childId, out var child))
                {
                    return false;
                }

                return compiledCondition(child);
            }, nextPage.PageKey));
            return this;
        }

        public Page Build()
        {
            return new Page(PageKey, _edges.ToList());
        }
    }
}
