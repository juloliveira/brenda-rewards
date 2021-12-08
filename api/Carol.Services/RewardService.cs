using Carol.Contracts;
using Carol.Core;
using Carol.Core.Services;
using Carol.Core.Services.Model;
using Carol.Data;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Carol.Services
{
    public class RewardService : IRewardService
    {
        private readonly CarolContext _context;

        public RewardService(CarolContext context)
        {
            _context = context;
        }

        public async Task<UserAward> ToAward(RewardUser rewardUser)
        {
            if (rewardUser is null) throw new ArgumentNullException(nameof(rewardUser));

            User user;
            Campaign campaign;
            using (var tx = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    campaign = await _context.Campaigns.FirstOrDefaultAsync(x => x.Id == rewardUser.CampaignId);
                    user = await _context.Users.SingleAsync(x => x.Id == rewardUser.UserId);

                    var transactionCampaign = campaign.RewardUser(user, rewardUser.VoucherId, rewardUser.Reward);
                    var transactionUser = user.ReceiveReward(campaign, transactionCampaign, rewardUser.Customer);

                    await _context.AddRangeAsync(new[] { transactionCampaign, transactionUser });
                    await _context.SaveChangesAsync();

                    await tx.CommitAsync();

                    return new UserAward(rewardUser.UserId, rewardUser.Reward, user.Balance, campaign.Title, campaign.Customer, user.FirebaseToken);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    throw ex;
                }
            }

            
        }
    }
}
