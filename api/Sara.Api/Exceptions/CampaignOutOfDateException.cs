using Sara.Contracts.Events;

namespace Sara.Api.Exceptions
{
    public class CampaignOutOfDateException : SaraRequestException
    {
        public CampaignOutOfDateException(CampaignRequest campaignRequest) 
            : base("Campanha não é mais válida.", campaignRequest) { }
    }
}
