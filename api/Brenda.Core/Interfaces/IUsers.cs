using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brenda.Core.Interfaces
{
    public interface IUsers
    {
        Task<BrendaUser> GetFullById(Guid id);
        Task<BrendaUser> GetById(Guid id);
        Task SaveChanges();
        Task<BrendaUser> GetByEmail(string v);
        Task<BrendaUser> GetCurrentAsync();
        Task<IEnumerable<BrendaUser>> GetActive();
    }
}
