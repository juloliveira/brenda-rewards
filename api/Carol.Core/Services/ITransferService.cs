using System.Threading.Tasks;

namespace Carol.Core.Services
{
    public interface ITransferService
    {
        Task<Transaction> Transfer(User from, User destination, double valeu);
    }
}
