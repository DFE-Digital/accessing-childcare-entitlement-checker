using System.Linq.Expressions;
using static AccessingChildcareEntitlementChecker.Web.IServiceCollectionExtensions;

namespace AccessingChildcareEntitlementChecker.Web.Services
{
    public class JourneyBuilder
    {
        private List<JourneyPageBuilder> _pages;

        public JourneyBuilder()
        {
            _pages = new List<JourneyPageBuilder>();
        }

        public JourneyPageBuilder Add<TController>(string url, string action)
        {
            var controllerTypeName = typeof(TController).Name;
            var controllerName = controllerTypeName.Replace("Controller", string.Empty);
            var page = new JourneyPageBuilder(controllerName, action, url);
            _pages.Add(page);
            return page;
        }

        public Journey Build(JourneyPageBuilder startPage)
        {
            var pages = _pages
                .Select(p => p.Build())
                .ToDictionary(p => p.PageKey, p => p);
            return new Journey(startPage.PageKey, pages);
        }
    }
}
