namespace Sara.Contracts.Events
{
    public class CampaignOutOfDate
    {
        protected CampaignOutOfDate() { }
        public CampaignOutOfDate(CampaignRequest campaignRequest)
        {
            CampaignRequest = campaignRequest;
        }

        public CampaignRequest CampaignRequest { get; protected set; }
    }
}
