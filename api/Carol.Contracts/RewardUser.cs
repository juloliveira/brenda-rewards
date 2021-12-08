using System;

namespace Carol.Contracts
{
    public class RewardUser
    {
        protected RewardUser() { }

        public RewardUser(Guid userId, string customer, Guid campaignId, Guid voucherId, double reward)
        {
            UserId = userId;
            CampaignId = campaignId;
            VoucherId = voucherId;
            Reward = reward;
            Customer = customer;
        }

        public Guid UserId { get; private set; }

        public Guid CampaignId { get; private set; }

        public Guid VoucherId { get; private set; }

        public string Customer { get; set; }

        public double Reward { get; protected set; }
    }
}
