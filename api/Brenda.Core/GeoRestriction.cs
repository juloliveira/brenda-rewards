using System;

namespace Brenda.Core
{
    public class GeoRestriction : Entity
    {
        public GeoRestriction() { }
        public GeoRestriction(CampaignDefinitions definitions, double radius, double latitude, double longitude)
        {
            Radius = radius;
            Latitude = latitude;
            Longitude = longitude;
            Definitions = definitions;
        }

        public double Radius { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Guid DefinitionsId { get; protected set; }
        public CampaignDefinitions Definitions { get; protected set; }
    }
}
