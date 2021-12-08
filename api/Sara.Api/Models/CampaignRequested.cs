using Brenda.Contracts.V1.Campaign;
using Newtonsoft.Json;
using Sara.Core;
using System;

namespace Sara.Api.Models
{
    public class CampaignRequested
    {
        public CampaignRequested(Voucher voucher, CampaignOnGoing campaign, Customer customer)
        {
            VoucherId = voucher.Id;
            Campaign = campaign;
            Logo = customer.LogoAvatar;
        }

        public Guid VoucherId { get; private set; }

        [JsonProperty("lo")]
        public string Logo { get; set; }

        public CampaignOnGoing Campaign { get; private set; }
    }
}
