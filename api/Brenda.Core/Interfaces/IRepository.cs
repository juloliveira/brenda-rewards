using Brenda.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brenda.Core.Interfaces
{
    public interface IRepository<TModel> where TModel : Entity
    {
        Task<IEnumerable<BaseEntity>> GetBaseAllAsync();

        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid id);
    }
}
