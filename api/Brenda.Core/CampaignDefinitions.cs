using System;
using System.Collections.Generic;

namespace Brenda.Core
{
    public class CampaignDefinitions : Entity
    {
        private List<GeoRestriction> _coordinatesAllowed;
        public DateTime? ValidationStart { get; set; }

        public DateTime? ValidationEnd { get; set; }

        public bool ValidateGeoLocation { get; set; }

        public IEnumerable<GeoRestriction> CoordinatesAllowed { get { return _coordinatesAllowed; } }


        internal bool WithinExpirationDate()
        {
            var utcNow = DateTime.UtcNow;
            return (ValidationStart <= utcNow && ValidationEnd <= utcNow);
        }

        public GeoRestriction AddGeoPermition(double radius, double latitude, double longitude)
        {
            if (_coordinatesAllowed == null)
                _coordinatesAllowed = new List<GeoRestriction>();

            ValidateGeoLocation = true;
            var restriction = new GeoRestriction(this, radius, latitude, longitude);
            _coordinatesAllowed.Add(restriction);

            return restriction;
        }
    }
}
