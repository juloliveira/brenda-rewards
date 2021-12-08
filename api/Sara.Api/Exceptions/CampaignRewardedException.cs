using Sara.Contracts.Events;

namespace Sara.Api.Exceptions
{
    public class CampaignRewardedException : SaraRequestException
    {
        public CampaignRewardedException(CampaignRequest campaignRequest) 
            : base("Você já foi recompensado por esta campanha.", campaignRequest) { }
    }
}
