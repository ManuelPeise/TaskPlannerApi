using Shared.Models.Dashboard;

namespace Logic.Dashboard.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardData> GetDashboardData(DateTime from, DateTime to);
    }
}
