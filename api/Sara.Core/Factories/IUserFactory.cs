using Sara.Contracts.Security;
using System.Threading.Tasks;

namespace Sara.Core.Factories
{
    public interface IUserFactory
    {
        Task<SaraUser> Create(UserRegister userRegister);
    }
}
