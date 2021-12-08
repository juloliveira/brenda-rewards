using Sara.Contracts.Events;

namespace Sara.Api.Exceptions
{
    public class CampaignInvalidLocationException : SaraRequestException
    {
        public CampaignInvalidLocationException(CampaignRequest campaignRequest) 
            : base("Sua localização não é permita para esta campanha", campaignRequest) { }
    }
}
