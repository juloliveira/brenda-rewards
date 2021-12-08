using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Contracts.V1.Requests.Campaigns
{
    public class Restriction
    {
        public Guid Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public int Radius { get; set; }
    }
}
