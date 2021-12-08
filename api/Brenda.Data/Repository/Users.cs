using Brenda.Core;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class Users : IUsers
    {
        private readonly BrendaContext _context;
        private readonly TenantInfo _tenantInfo;

        public Users(BrendaContext context, TenantInfo tenantInfo)
        {
            _context = context;
            _tenantInfo = tenantInfo;
        }

        public async Task<IEnumerable<BrendaUser>> GetActive()
        {
            return await _context.Users
                .Where(x => x.CustomerId == _tenantInfo.CustomerId && !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<BrendaUser> GetByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).SingleOrDefaultAsync();
        }

        public async Task<BrendaUser> GetById(Guid id)
        {
            return await _context.Users
                .Where(x => x.Id == id).SingleOrDefaultAsync();
        }

        public async Task<BrendaUser> GetCurrentAsync()
        {
            return await _context.Users
                .Where(x => x.Id == _tenantInfo.UserId && x.CustomerId == _tenantInfo.CustomerId)
                .SingleAsync();
        }

        public async Task<BrendaUser> GetFullById(Guid id)
        {
            var query = _context.Users
                .Where(x => x.Id == id);

            return await query.SingleOrDefaultAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
