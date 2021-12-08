using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class Actions : EFCoreRepository<Action>, IActions
    {
        public Actions(BrendaContext context, TenantInfo tenantInfo) : base(context, tenantInfo) { }

        public override Task<IEnumerable<Action>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public override async Task<IEnumerable<BaseEntity>> GetBaseAllAsync() => 
            await _dbSet
                    .Select(x => new BaseEntity(x.Id, x.Name))
                    .ToListAsync();
    }
}
