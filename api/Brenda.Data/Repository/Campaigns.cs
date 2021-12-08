using Brenda.Contracts.V1.Requests;
using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class Campaigns : EFCoreRepository<Campaign>, ICampaigns
    {
        private readonly IQueryable<Campaign> _query;

        public Campaigns(BrendaContext context, TenantInfo tenantInfo) : base(context, tenantInfo) 
        {
            _query = _dbSet.Where(x => x.CustomerId == tenantInfo.CustomerId);
        }

        public async Task<IEnumerable<Campaign>> FindByName(string q)
        {
            return await _dbSet
                .Include(x => x.Action)
                .Where(x => 
                    x.CustomerId == _tenantInfo.CustomerId && 
                    x.Title.StartsWith(q, StringComparison.OrdinalIgnoreCase) &&
                    x.ActionId != new Guid(Brenda.Core.Identifiers.Actions.Challenge)) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Campaign>> GetActive()
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Status != CampaignStatus.Archived)
                .Include(x => x.Customer)
                .Include(x => x.Definitions)
                .Include(x => x.Action)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        public override Task<IEnumerable<Campaign>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<BaseEntity>> GetBaseAllAsync()
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId)
                .Select(campaign => new BaseEntity(campaign.Id, campaign.Title))
                .ToListAsync();
        }

        public override async Task<Campaign> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(x => x.Id == id && x.CustomerId == _tenantInfo.CustomerId)
                .Include(x => x.Campaigns)
                    .ThenInclude(x => x.Action)
                .Include(x => x.Customer)
                .Include(x => x.Definitions)
                    .ThenInclude(x => x.CoordinatesAllowed)
                .Include(x => x.Action)
                .Include(x => x.Asset)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Options)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<BaseEntity>> GetByStatusAsync(CampaignStatus status)
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Status == status)
                .Select(x => new BaseEntity(x.Id, x.Title))
                .ToListAsync();
        }

        public async Task<Campaign> GetByTag(string tag)
        {
            return await _dbSet
                .Include(x => x.Customer)
                .Include(x => x.Definitions)
                .FirstOrDefaultAsync(x => x.Tag == tag);
        }

        public async Task<IEnumerable<Campaign>> GetChallengeCampaigns(Campaign campaign)
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Id == campaign.Id)
                .SelectMany(x => x.Campaigns)
                .ToListAsync();
        }

        public async Task<CampaignDefinitions> GetDefinitionsById(Guid id)
        {
            return await _dbSet
                .Where(x => x.Id == id)
                .Include(x => x.Definitions)
                .ThenInclude(x => x.CoordinatesAllowed)
                .Select(x => x.Definitions)
                .FirstOrDefaultAsync();
        }

        public async Task<Campaign> GetOnGoingById(Guid id)
        {
            return await _query
                .Where(x => x.Id == id && x.Status == CampaignStatus.OnGoing)
                .Include(x => x.Customer)
                .Include(x => x.Definitions)
                    .ThenInclude(x => x.CoordinatesAllowed)
                .Include(x => x.Action)
                .Include(x => x.Asset)
                .SingleOrDefaultAsync();
        }

        public async Task<Campaign> GetByIdToPublishAsync(Guid campaignId)
        {
            return await _query
                .Where(x => x.CustomerId == _tenantInfo.CustomerId 
                        && x.Id == campaignId 
                        && x.Status != CampaignStatus.Archived)
                .Include(x => x.Campaigns)
                    .ThenInclude(x => x.Definitions)
                .Include(x => x.Campaigns)
                    .ThenInclude(x => x.Action)
                .Include(x => x.Campaigns)
                    .ThenInclude(x => x.Asset)
                        .ThenInclude(x => x.Questions)
                            .ThenInclude(x => x.Options)
                .Include(x => x.Customer)
                .Include(x => x.Definitions)
                    .ThenInclude(x => x.CoordinatesAllowed)
                .Include(x => x.Action)
                .Include(x => x.Asset)
                    .ThenInclude(x => x.Questions)
                        .ThenInclude(x => x.Options)
                .Include(x => x.UrlActions)
                .SingleOrDefaultAsync();
        }

        public async Task<GeoRestriction> GetRestrictionById(Guid id)
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId)
                .Include(x => x.Definitions)
                .ThenInclude(x => x.CoordinatesAllowed)
                .SelectMany(x => x.Definitions.CoordinatesAllowed)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetTagAsync(Guid id)
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Id == id)
                .Select(x => x.Tag)
                .SingleAsync();
        }

        public bool RemoveRestriction(GeoRestriction restriction)
        {
            var result = _context.Remove(restriction);
            return result.State == EntityState.Deleted;
        }

        public async Task<int> SetAsset(Guid campaignId, Guid assetId)
        {
            var campaign = await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Id == campaignId)
                .SingleAsync();
            campaign.AssetId = assetId;
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<UrlAction> GetUrlActionAsync(Guid campaingId, Guid urlActionId)
        {
            var query = await _context.Campaigns
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Id == campaingId)
                .Include(x => x.UrlActions)
                .Select(x => x.UrlActions.Where(x => x.Id == urlActionId))
                .FirstOrDefaultAsync();

            return query.FirstOrDefault();
        }

        public bool RemoveUrlAction(UrlAction urlAction)
        {
            var result = _context.Remove(urlAction);
            return result.State == EntityState.Deleted;
        }
    }
}
