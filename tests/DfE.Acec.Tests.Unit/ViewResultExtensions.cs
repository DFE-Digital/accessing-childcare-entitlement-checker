using Microsoft.AspNetCore.Mvc;

namespace Dfe.Acec.Tests.Unit;

internal static class ViewResultExtensions
{
    public static T Model<T>(this ViewResult viewResult) => Assert.IsType<T>(viewResult.Model);
}
