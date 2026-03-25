using Shared.Enums;

namespace Shared.Models.Dashboard
{
    public class LogMessageDashboardTile
    {
        public string TitleResourceKey { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Info { get; set; }
        public int Warning { get; set; }
        public int Error { get; set; }
    }
}
