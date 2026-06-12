using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

namespace AccessingChildcareEntitlementChecker.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddJourneyServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IDateTimeFactory, DateTimeFactory>();
        services.AddScoped<ITodayFactory, UkTodayFactory>();
        services.AddScoped<IJourneySession, JourneySession>();
        services.AddScoped(sp =>
        {
            var journeySession = sp.GetRequiredService<IJourneySession>();
            return journeySession.Get();
        });

        // Register Navigation Services & Strategy Steps
        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<IWorkflowStep, HomeWorkflowSteps>();
        services.AddScoped<IWorkflowStep, IntroductionWorkflowSteps>();
        services.AddScoped<IWorkflowStep, BornChildDetailsWorkflowSteps>();
        services.AddScoped<IWorkflowStep, ExpectedChildDetailsWorkflowSteps>();
        services.AddScoped<IWorkflowStep, SummaryWorkflowSteps>();
        services.AddScoped<IWorkflowStep, UserWorkflowSteps>();
        services.AddScoped<IWorkflowStep, PartnerWorkflowSteps>();

        return services;
    }
}
