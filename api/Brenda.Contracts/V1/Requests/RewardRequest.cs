using System;

namespace Brenda.Contracts.V1.Requests
{
    public class RewardRequest
    {
        public Guid Ticket { get; set; }
        public string Tag { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}