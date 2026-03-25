using Logic.Dashboard.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Dashboard;
using System.Globalization;

namespace Service.Api.ApiControllers.Dashboard
{
    public class DashboardController : ApiControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService) : base()
        {
            _dashboardService = dashboardService;
        }

        [JwtAuthentication]
        [HttpGet(Name = "GetDashboardData")]
        public async Task<DashboardData> GetDashboardData([FromQuery] string from, string to)
        {
            var fromDate = DateTime.ParseExact(from, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var toDate = DateTime.ParseExact(to, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            return await _dashboardService.GetDashboardData(fromDate, toDate);
        }
    }
}
