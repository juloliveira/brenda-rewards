using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Contracts.V1.Responses
{
    public class CampaignResponse
    {
        public string Tag { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public string ResourceUri { get; set; }
        public string Ticket { get; set; }
    }
}
