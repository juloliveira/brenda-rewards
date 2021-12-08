using Brenda.Core.DTO;
using System;
using System.Threading.Tasks;

namespace Brenda.Core.Interfaces
{
    public interface ICustomers : IRepository<Customer>
    {
        Task<AccountOverview> GetAccountStatementAsync();
        Task<Customer> GetCurrentCustomerAsync();
        Task<AccountActivity> GetAccountActivityByIdAsync(Guid id);
        Task<bool> HasDocumentAsync(string companyDocument);
    }
}
