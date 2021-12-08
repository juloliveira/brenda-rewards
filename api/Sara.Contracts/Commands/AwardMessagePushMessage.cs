using Sara.Contracts.Security;
using System;

namespace Sara.Contracts.Commands
{
    public class AwardPushMessage
    {
        public Guid UserId { get; set; }
        public double Reward { get; set; }
        public double Balance { get; set; }
        public string CampaignTitle { get; set; }
        public string CampaignCustomer { get; set; }
    }
}
