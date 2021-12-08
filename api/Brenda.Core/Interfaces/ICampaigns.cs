using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Brenda.Contracts.V1.Requests;
using Brenda.Core.DTO;

namespace Brenda.Core.Interfaces
{
    public interface ICampaigns : IRepository<Campaign>
    {
        Task<Campaign> GetByTag(string tag);
        Task<IEnumerable<Campaign>> GetActive();
        Task<CampaignDefinitions> GetDefinitionsById(Guid id);
        Task<GeoRestriction> GetRestrictionById(Guid id);
        
        Task<int> SetAsset(Guid campaignId, Guid assetId);
        Task<string> GetTagAsync(Guid id);
        Task<IEnumerable<BaseEntity>> GetByStatusAsync(CampaignStatus status);
        Task<Campaign> GetByIdToPublishAsync(Guid campaignId);
        Task<Campaign> GetOnGoingById(Guid id);
        Task<IEnumerable<Campaign>> FindByName(string q);
        Task<IEnumerable<Campaign>> GetChallengeCampaigns(Campaign campaign);
        Task<UrlAction> GetUrlActionAsync(Guid campaingId, Guid urlActionId);

        bool RemoveRestriction(GeoRestriction restriction);
        bool RemoveUrlAction(UrlAction urlAction);
    }
}
