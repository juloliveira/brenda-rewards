using Sara.Contracts.Commands;
using System.Threading.Tasks;

namespace Sara.Api.Infrasctructure
{
    public interface IPushMessage
    {
        Task<string> AwardMessage(AwardPushMessage push, string token);
        Task<string> TransferReceivedMessage(TransferReceivedPushMessage push, string token);
        Task<string> UpdateBalance(double balance, string token);
    }
}
