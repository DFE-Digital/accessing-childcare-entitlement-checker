using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.Web
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddJourneyServices(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeFactory, DateTimeFactory>();
            services.AddScoped<IJourneySession, JourneySession>();
            services.AddScoped(sp =>
            {
                var journeySession = sp.GetRequiredService<IJourneySession>();
                return journeySession.Get();
            });

            return services;
        }
    }
}
