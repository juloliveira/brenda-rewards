namespace Sara.Contracts.Events
{
    public class CampaignNotFound
    {
        protected CampaignNotFound() { }
        public CampaignNotFound(CampaignRequest campaignRequest)
        {
            CampaignRequest = campaignRequest;
        }

        public CampaignRequest CampaignRequest { get; protected set; }
    }
}
