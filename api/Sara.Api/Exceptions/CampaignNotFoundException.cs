using Sara.Contracts.Events;

namespace Sara.Api.Exceptions
{
    public class CampaignNotFoundException : SaraRequestException
    {
        public CampaignNotFoundException(CampaignRequest campaignRequest) 
            : base("Sua localização não é permita para esta campanha", campaignRequest) { }
    }
}
