using Logic.Tasks.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Tasks.DI
{
    public static class TaskServiceRegistration
    {
        public static void AddTaskServices(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
        }
    }
}