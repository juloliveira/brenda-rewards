using System;

namespace Sara.Contracts.Events
{
    public class CampaignRequest
    {
        public Guid UserId { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? CustomerId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Acceleration { get; set; }
        public double Altitude { get; set; }
        public double Speed { get; set; }
        public double SpeedAccuracy { get; set; }
        public double Heading { get; set; }
        public long RequestedAt { get; set; }
        public string DeviceId { get; set; }
        public string DeviceData { get; set; }
    }
}
