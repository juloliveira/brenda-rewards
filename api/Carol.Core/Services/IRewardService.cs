using Carol.Contracts;
using Carol.Core.Services.Model;
using System.Threading.Tasks;

namespace Carol.Core.Services
{
    public interface IRewardService
    {
        Task<UserAward> ToAward(RewardUser rewardUser);
    }
}
