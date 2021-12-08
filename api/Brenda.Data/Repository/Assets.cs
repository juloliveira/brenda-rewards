using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class Assets : EFCoreRepository<Asset>, IAssets
    {
        public Assets(BrendaContext context, TenantInfo tenantInfo) : base(context, tenantInfo) { }

        public async Task<IEnumerable<Asset>> FindByIdOrName(string search, string actionTag)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Action)
                .Where(x =>
                        x.CustomerId == _tenantInfo.CustomerId &&
                        x.Action.Tag == actionTag &&
                        x.Enable &&
                        x.Title.StartsWith(search, StringComparison.OrdinalIgnoreCase)
                    )
                .Take(30)
                .ToListAsync();
        }

        public async Task<IEnumerable<Asset>> GetActiveAsync()
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Enable)
                .ToListAsync();
        }

        public async override Task<IEnumerable<Asset>> GetAllAsync()
        {
            return await _dbSet.Where(x => x.CustomerId == _tenantInfo.CustomerId).ToListAsync();
        }

        public override async Task<IEnumerable<BaseEntity>> GetBaseAllAsync() =>
            await _dbSet
            .Where(x => x.CustomerId == _tenantInfo.CustomerId)
            .Select(x => new BaseEntity(x.Id, x.Title))
            .ToListAsync();

        public override async Task<Asset> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(x => x.Action)
                .Include(x => x.Questions)
                .Where(x => x.Id == id && x.CustomerId == _tenantInfo.CustomerId)
                .SingleAsync();
        }

        public async Task<Quiz> GetQuestionByIdAsync(Guid assetId, Guid questionId)
        {
            return await _dbSet
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .Where(x =>
                    x.CustomerId == _tenantInfo.CustomerId &&
                    x.Id == assetId )
                .SelectMany(x=>x.Questions)
                .Where(x => x.Id == questionId)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Quiz>> GetQuizByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && x.Id == id)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Options)
                .SelectMany(x => x.Questions)
                .OrderBy(x => x.Order)
                .ToListAsync();
        }

        public void Remove(Quiz option)
        {
            _context.Remove(option);
        }
    }
}
