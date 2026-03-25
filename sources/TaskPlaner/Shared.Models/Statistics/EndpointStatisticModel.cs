using Shared.Enums;

namespace Shared.Models.Statistics
{
    public class EndpointStatisticModel
    {
        public EndpointEnum Endpoint { get; set; }
        public DateTime TimeStamp { get; set; }
        public double RequestTime { get; set; }
    }
}
