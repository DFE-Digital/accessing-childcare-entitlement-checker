using Dfe.Acec.Web.Extensions;
using Dfe.Acec.Web.Models;

namespace Dfe.Acec.Web.Tests.Unit.Extensions;

public class NationalityOptionExtensionsTests
{
    [Theory]
    [InlineData(new[] { NationalityOption.BritishOrIrishCitizen }, false)]
    [InlineData(new[] { NationalityOption.BritishOrIrishCitizen, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland }, false)]
    [InlineData(new[] { NationalityOption.BritishOrIrishCitizen, NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, NationalityOption.CitizenOfADifferentCountry }, false)]
    [InlineData(new[] { NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, NationalityOption.CitizenOfADifferentCountry }, true)]
    [InlineData(new[] { NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland }, true)]
    [InlineData(new[] { NationalityOption.CitizenOfADifferentCountry }, false)]
    public void NeedsSettledStatusAnswerReturnsExpectedResult(NationalityOption[] nationalityOptions, bool expectedResult)
    {
        List<NationalityOption> options = [.. nationalityOptions];
        var result = options.NeedsSettledStatusAnswer();
        Assert.Equal(expectedResult, result);
    }
}
