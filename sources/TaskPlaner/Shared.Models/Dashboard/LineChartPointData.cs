namespace Shared.Models.Dashboard
{
    public class LineChartPointData
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public double Avg { get; set; }
        public string Color { get; set; } = string.Empty;

    }
}