using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sara.Core.Interfaces
{

    public interface IRepository<TModel> where TModel : Entity
    {
        Task<TModel> GetByIdAsync(Guid id);
    }
}
