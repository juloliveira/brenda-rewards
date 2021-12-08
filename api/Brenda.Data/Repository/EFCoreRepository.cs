using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public abstract class EFCoreRepository<TModel> : IRepository<TModel> where TModel : Entity
    {
        protected internal readonly BrendaContext _context;

        protected internal DbSet<TModel> _dbSet;

        protected internal TenantInfo _tenantInfo;

        public EFCoreRepository(BrendaContext context, TenantInfo tenantInfo)
        {
            _context = context;
            _dbSet = _context.Set<TModel>();
            _tenantInfo = tenantInfo;
        }

        public virtual async Task<TModel> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }
        
        public abstract Task<IEnumerable<BaseEntity>> GetBaseAllAsync();

        public abstract Task<IEnumerable<TModel>> GetAllAsync();
        
    }
}
