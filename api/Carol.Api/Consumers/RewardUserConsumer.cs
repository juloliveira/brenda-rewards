using Carol.Contracts;
using Carol.Core.Services;
using MassTransit;
using Sara.Contracts.Commands;
using System.Threading.Tasks;

namespace Carol.Api.Consumers
{
    public class RewardUserConsumer : IConsumer<RewardUser>
    {
        private readonly IRewardService _rewardService;
        private readonly IBus _bus;

        public RewardUserConsumer(IRewardService rewardService, IBus bus)
        {
            _rewardService = rewardService;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<RewardUser> context)
        {
            var userAward = await _rewardService.ToAward(context.Message);

            await _bus.Publish(new AwardPushMessage
            {
                UserId = userAward.UserId,
                Reward = userAward.Reward,
                Balance = userAward.Balance,
                CampaignTitle = userAward.CampaignTitle,
                CampaignCustomer = userAward.CampaignCustomer
            });
        }
    }
}
