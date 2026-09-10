using Dfe.Acec.Web.Models;

namespace Dfe.Acec.Web.Extensions;

public static class NationalityOptionsListExtensions
{
    public static bool IsBritishOrIrishCitizen(this List<NationalityOption> nationalityOptions)
    {
        return nationalityOptions.Contains(NationalityOption.BritishOrIrishCitizen);
    }

    public static bool IsCitizenOfAnEuCountryEeaCountryOrSwitzerland(this List<NationalityOption> nationalityOptions)
    {
        return nationalityOptions.Contains(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland);
    }

    public static bool NeedsSettledStatusAnswer(this List<NationalityOption> nationalityOptions)
    {
        return IsCitizenOfAnEuCountryEeaCountryOrSwitzerland(nationalityOptions)
            && !IsBritishOrIrishCitizen(nationalityOptions);
    }
}
