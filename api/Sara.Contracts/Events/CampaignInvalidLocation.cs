namespace Sara.Contracts.Events
{
    public class CampaignInvalidLocation
    {
        protected CampaignInvalidLocation() { }
        public CampaignInvalidLocation(CampaignRequest campaignRequest)
        {
            CampaignRequest = campaignRequest;
        }

        public CampaignRequest CampaignRequest { get; protected set; }
    }
}
