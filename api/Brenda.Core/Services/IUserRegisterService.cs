using Brenda.Contracts.V1.Requests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Core.Services
{
    public interface IUserRegisterService
    {
        Task ConfirmEmail(string token, string email);

        Task Register(string companyName, string companyDocument, string name, string email, string password, string role);
        Task Register(Customer customer, string name, string email, string role);
    }
}
