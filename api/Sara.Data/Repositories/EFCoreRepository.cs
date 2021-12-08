using Microsoft.EntityFrameworkCore;
using Sara.Core;
using Sara.Core.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Data.Repositories
{
    public abstract class EFCoreRepository<TModel> : IRepository<TModel> where TModel : Entity
    {
        protected internal DbSet<TModel> DbSet { get; private set; }
        public EFCoreRepository(SaraContext context)
        {
            DbSet = context.Set<TModel>();
        }

        public async Task<TModel> GetByIdAsync(Guid id)
        {
            return await DbSet.Where(x => x.Id == id).SingleAsync();
        }
    }
}
