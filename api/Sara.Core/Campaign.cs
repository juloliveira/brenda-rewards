using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Core
{
    public class Campaign : Entity
    {
        private List<Campaign> _campaigns;
        protected Campaign() { }
        public Campaign(Guid id, Guid customerId, string version, string content) : base(id)
        {
            _campaigns = new List<Campaign>();
            Version = version;
            Content = content;
            CustomerId = customerId;
        }

        public Guid CustomerId { get; protected set; }

        public string Content { get; protected set; }

        public string Version { get; protected set; }

        public Guid? ChallengeId { get; protected set; }
        public Campaign Challenge { get; protected set; }

        public IReadOnlyCollection<Campaign> Campaigns => _campaigns.AsReadOnly();

        internal void SetChallenge(Campaign campaign)
        {
            ChallengeId = campaign.Id;
        }

        public void AddCampaign(Campaign campaign)
        {
            if (campaign == null) throw new ArgumentNullException(nameof(campaign));

            campaign.SetChallenge(this);
            _campaigns.Add(campaign);
        }
    }
}
