using Sara.Contracts.Events;
using System;

namespace Sara.Api.Exceptions
{
    public abstract class SaraRequestException : Exception, IApiException
    {
        public SaraRequestException(string message, CampaignRequest campaignRequest) : base(message)
        {
            CampaignRequest = campaignRequest;
            Status = 403;
        }

        public CampaignRequest CampaignRequest { get; set; }

        public int Status { get; internal set; }
    }
}
