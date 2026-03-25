using Logic.Dashboard.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Dashboard.DI
{
    public static class DashboardServiceRegistration
    {
        public static void AddDashboardServices(this IServiceCollection services)
        {
            services.AddScoped<IEndpointStatisticService, EndpointStatisticService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
