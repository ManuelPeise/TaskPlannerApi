using Shared.Enums;

namespace Data.Entities.Statistics
{
    public class EndpointStatisticEntity : AEntityBase
    {
        public EndpointEnum Endpoint { get; set; }
        public DateTime TimeStamp { get; set; }
        public double ElapsedSeconds { get; set; }
    }
}
