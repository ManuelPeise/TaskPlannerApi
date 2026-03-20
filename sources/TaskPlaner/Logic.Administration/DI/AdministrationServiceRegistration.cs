using Logic.Administration.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Administration.DI
{
    public static class AdministrationServiceRegistration
    {
        public static void AddAdministrationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAdministration, UserAdministration>();
            services.AddScoped<IUserAuthentication, UserAuthentication>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }
    }
}
