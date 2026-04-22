using Microsoft.AspNetCore.Mvc;

namespace AccessingChildcareEntitlementChecker.UnitTests
{
    internal static class ViewResultExtensions
    {
        public static T Model<T>(this ViewResult viewResult) => Assert.IsType<T>(viewResult.Model);
    }
}
