using Brenda.Core;
using Brenda.Core.DTO;
using Brenda.Core.Interfaces;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class Customers : EFCoreRepository<Customer>, ICustomers
    {
        public Customers(BrendaContext context, TenantInfo tenantInfo) : base(context, tenantInfo) { }

        public async Task<AccountActivity> GetAccountActivityByIdAsync(Guid id)
        {
            return await _dbSet
                .Where(x => x.Id == _tenantInfo.CustomerId)
                .SelectMany(x => x.AccountStatement)
                .Include(x => x.User)
                .Include(x => x.Customer)
                .Where(x => x.Id == id)
                .SingleAsync();
        }

        public async Task<AccountOverview> GetAccountStatementAsync()
        {
            return await _dbSet
                .Where(x => x.Id == _tenantInfo.CustomerId)
                .Include(x => x.AccountStatement)
                .Include(x => x.Settings)
                .Select(x => new AccountOverview 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Balance = x.Balance,
                    HasLogo = x.Settings.HasLogo,
                    Email = x.Settings.Email,
                    LogoAvatar = x.Settings.LogoAvatar,
                    AccountStatement = 
                        x.AccountStatement
                            .OrderByDescending(x => x.CreatedAt)
                            .Select(a => new AccountOverview.AccountActivity 
                            { 
                                Id = a.Id,
                                Description = a.Description,
                                Reference = a.Reference,
                                Reason = a.Reason,
                                Value = a.Value,
                                CreatedAt = a.CreatedAt
                            })
                    
                })
                .SingleAsync();
        }

        public override Task<IEnumerable<Customer>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<BaseEntity>> GetBaseAllAsync() =>
            await _dbSet
                .Where(x => x.Id == _tenantInfo.CustomerId)
                .Select(x => new BaseEntity(x.Id, x.Name))
                .ToListAsync();

        public async Task<Customer> GetCurrentCustomerAsync() =>
            await _dbSet
                    .Include(x => x.Settings)
                    .Where(x => x.Id == _tenantInfo.CustomerId).SingleAsync();

        public async Task<bool> HasDocumentAsync(string companyDocument)
        {
            return await _dbSet.AnyAsync(x => x.Document == companyDocument);
        }

        
    }
}
