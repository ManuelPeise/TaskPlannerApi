using Logic.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Shared.DI
{
    public static class SharedServiceRegistration
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
            services.AddScoped<ITaskUnitOfWork, TaskUnitOfWork>();
            services.AddScoped<IEmailClient, EmailClient>();
        }
    }
}
