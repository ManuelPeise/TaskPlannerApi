namespace Shared.Models.Dashboard
{
    public class DashboardData
    {
        public LogMessageDashboardTile? LogMessageTile { get; set; }
        public UserDashboardTile? UserTile { get; set; }
        public DashboardTileEndpointPerformanceDataSet? TaskControllerPerformanceData { get; set; }
    }
}
