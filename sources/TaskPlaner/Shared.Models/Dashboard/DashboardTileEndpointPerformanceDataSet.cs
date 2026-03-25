using Shared.Enums;

namespace Shared.Models.Dashboard
{
    public class DashboardTileEndpointPerformanceDataSet
    {
        public string TitleResourceKey { get; set; } = string.Empty;
        public List<DashboardEndpointPerformanceData> EndpointPerformanceData { get; set; } = new List<DashboardEndpointPerformanceData>();
    }

    public class DashboardEndpointPerformanceData
    {
        public string EndpointLabel { get; set; } = string.Empty;
        public string ColorKey { get; set; } = string.Empty;
        public List<LineChartPointData> PerformanceData { get; set; } = new List<LineChartPointData>();

    }
}
