using Newtonsoft.Json;
using System;

namespace Brenda.Contracts.V1.Campaign
{
    public class CampaignRestriction
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }

        [JsonProperty("rad")]
        public int Radius { get; set; }

        public double DistanceTo(double toLatitude, double toLongitude)
        {
            double rlat1 = Math.PI * toLatitude / 180;
            double rlat2 = Math.PI * this.Latitude / 180;
            double theta = toLongitude - this.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344; // Meters -> default
        }
    }
}
