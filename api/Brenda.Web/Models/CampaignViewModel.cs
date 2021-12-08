using Brenda.Core;
using System;
using System.Collections.Generic;

namespace Brenda.Web.Models
{
    public class CampaignViewModel : Contracts.V1.Requests.CampaignForm
    {
        public string Tag { get; set; }
        public double Balance { get; set; }

        public string ActionImage { get; set; }
        public string ActionName { get; set; }
        public string ActionTag { get; set; }

        public Guid? AssetId { get; set; }
        public string AssetTitle { get; set; }
        public string AssetResource { get; set; }

        public Guid? ChallengeId { get; set; }

        

        public CampaignStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public CampaignViewModel[] Campaigns { get; set; }

        public UrlActionViewModel[] UrlActions { get; set; }
    }

}
