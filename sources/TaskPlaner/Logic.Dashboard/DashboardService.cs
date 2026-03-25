using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities.User;
using Logic.Dashboard.Interfaces;
using Logic.Shared.Interfaces;
using Shared.Enums;
using Shared.Models.Dashboard;

namespace Logic.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IEndpointStatisticService _endpointStatisticService;
        private readonly ILogger<DashboardService> _logger;
        private readonly IDbRepositoryBase<UserEntity> _userRepository;

        public DashboardService(DatabaseContext dbContext, IEndpointStatisticService endpointStatisticService, ILogger<DashboardService> logger)
        {
            _endpointStatisticService = endpointStatisticService;
            _userRepository = new DbRepositoryBase<UserEntity>(dbContext);
            _logger = logger;
        }

        public async Task<DashboardData> GetDashboardData(DateTime from, DateTime to)
        {
            var dashboardTileModel = new DashboardData();

            try
            {
                dashboardTileModel.LogMessageTile = await GetLogMessageDataSet();
                dashboardTileModel.UserTile = await GetUserDataSet();
                dashboardTileModel.TaskControllerPerformanceData = await GetEndpointPerformanceDataSet(from, to);

            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("Error retrieving dashboard data.", LogMessageTypeEnum.Error, exception);
            }

            return dashboardTileModel;
        }

        private async Task<DashboardTileEndpointPerformanceDataSet> GetEndpointPerformanceDataSet(DateTime from, DateTime to)
        {
            var fromStartOfDay = from.Date;
            var toEndOfDay = to.Date.AddDays(1).AddTicks(-1);            
            var endpointStatistics = await _endpointStatisticService.GetEndpointStatisticsAsync(fromStartOfDay, toEndOfDay);

            var stats = await _endpointStatisticService.GetEndpointStatisticsAsync(
                 fromStartOfDay,
                 toEndOfDay.AddDays(1).AddTicks(-1));

            var days = Enumerable
                .Range(0, (toEndOfDay - fromStartOfDay).Days + 1)
                .Select(offset => fromStartOfDay.AddDays(offset))
                .ToList();

            var endpoiontList = new List<EndpointEnum>
            {
                EndpointEnum.AddTask,
                EndpointEnum.UpdateTaskBase,
                EndpointEnum.UpdateTask,
                EndpointEnum.DeleteTask
            };

            var endpoints = endpointStatistics.Where(e => endpoiontList.Contains(e.Endpoint)).Select(e => e.Endpoint).Distinct().ToList();

            var performanceData = new List<DashboardEndpointPerformanceData>();

            foreach (var endpoint in endpoints)
            {
                var color = GetTaskEndpointColor(endpoint);
                var label = GetEndpointResourceKey(endpoint);

                var points = new List<LineChartPointData>();

                foreach (var day in days)
                {
                    var dayStats = stats
                        .Where(e => e.Endpoint == endpoint && e.TimeStamp.Date == day.Date)
                        .ToList();

                    var count = dayStats.Count;
                    var avg = count > 0 ? dayStats.Sum(e => e.RequestTime)/count : 0;

                    points.Add(new LineChartPointData
                    {
                        Date = day,
                        Count = count,
                        Avg = avg,
                        Color = color
                    });
                }

                performanceData.Add(new DashboardEndpointPerformanceData
                {
                    EndpointLabel = label,
                    ColorKey = color,
                    PerformanceData = points
                });
            }
            
            return new DashboardTileEndpointPerformanceDataSet
            {
                TitleResourceKey = "labelTaskControllerPerformance",
                EndpointPerformanceData = performanceData
            };
        }

        private async Task<LogMessageDashboardTile> GetLogMessageDataSet()
        {
            var logMessageEntities = await _logger.GetLogMessages();

            return new LogMessageDashboardTile
            {
                TitleResourceKey = "labelLogMessages",
                Count = logMessageEntities.Count(),
                Error = logMessageEntities.Count(x => x.LogMessageType == LogMessageTypeEnum.Error),
                Info = logMessageEntities.Count(x => x.LogMessageType == LogMessageTypeEnum.Info),
                Warning = logMessageEntities.Count(x => x.LogMessageType == LogMessageTypeEnum.Warn),
            };
        }

        private async Task<UserDashboardTile> GetUserDataSet()
        {
            try
            {
                var userEntities = await _userRepository.GetAll();

                return new UserDashboardTile
                {
                    TitleResourceKey = "labelUsers",
                    Count = userEntities.Count(),
                    Active = userEntities.Count(x => x.IsActive)
                };
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("Error retrieving endpoint performance data.", LogMessageTypeEnum.Error, exception);
                throw;
            }
        }

        private string GetTaskEndpointColor(EndpointEnum endpointEnum)
        {
            switch (endpointEnum)
            {
                case EndpointEnum.GetTaskDetailsPageModel:
                    return "blue";
                case EndpointEnum.AddTask:
                    return "orange";
                case EndpointEnum.UpdateTaskBase:
                    return "purple";
                case EndpointEnum.UpdateTask:
                    return "yellow";
                case EndpointEnum.DeleteTask:
                    return "red";
                case EndpointEnum.GetTaskPageModel:
                    return "cyan";
                default: return "green";
            }
        }

        private string GetEndpointResourceKey(EndpointEnum endpointEnum)
        {
            switch (endpointEnum)
            {
                case EndpointEnum.AddTask:
                    return "labelAddTask";
                case EndpointEnum.UpdateTask:
                    return "labelUpdateTask";
                case EndpointEnum.DeleteTask:
                    return "labelDeleteTask";
                case EndpointEnum.UpdateTaskBase:
                    return "labelUpdateTaskBase";
                default: return "labelUnknownEndpoint";
            }
        }
    }
}
