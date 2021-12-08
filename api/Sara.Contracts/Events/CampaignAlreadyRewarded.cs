namespace Sara.Contracts.Events
{
    public class CampaignAlreadyRewarded
    {
        protected CampaignAlreadyRewarded() { }

        public CampaignAlreadyRewarded(CampaignRequest campaignRequest)
        {
            CampaignRequest = campaignRequest;
        }

        public CampaignRequest CampaignRequest { get; protected set; }
    }
}
