using Brenda.Core.Interfaces;
using Brenda.Data.Repository;
using Brenda.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Data
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task CommitAsync();

        IAssets Assets { get; }
        IActions Actions { get; }
        ICustomers Customers { get; }
        ICampaigns Campaigns { get; }
        IUsers Users { get; }

        Task AddAsync<BrendaModel>(BrendaModel model);
        void Attach<BrendaModel>(BrendaModel model);
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BrendaContext _context;
        private readonly TenantInfo _tenantInfo;

        private IAssets _assets;
        private IActions _actions;
        private ICustomers _customers;
        private ICampaigns _campaigns;
        private IUsers _users;

        public UnitOfWork(BrendaContext context, TenantInfo tenantInfo)
        {
            _context = context;
            _tenantInfo = tenantInfo;
        }

        public IAssets Assets => _assets = _assets ?? new Assets(_context, _tenantInfo);
        public IActions Actions => _actions = _actions ?? new Actions(_context, _tenantInfo);
        public ICustomers Customers => _customers = _customers ?? new Customers(_context, _tenantInfo);
        public ICampaigns Campaigns => _campaigns = _campaigns ?? new Campaigns(_context, _tenantInfo);
        public IUsers Users => _users = _users ?? new Users(_context, _tenantInfo);

        public async Task AddAsync<BrendaModel>(BrendaModel model)
        {
            await _context.AddAsync(model);
        }

        public void Attach<BrendaModel>(BrendaModel model)
        {
            _context.Attach(model);
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
