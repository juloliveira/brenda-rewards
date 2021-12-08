using Brenda.Contracts.V1.Campaign;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sara.Core;
using Sara.Data;
using System;
using System.Threading.Tasks;

namespace Sara.Api.Consumers
{
    public class ChallengeOnGoingConsumer : IConsumer<ChallengeOnGoing>
    {
        private readonly SaraContext _context;

        public ChallengeOnGoingConsumer(SaraContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ChallengeOnGoing> bus)
        {
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
            try
            {
                var challenge = new Campaign(
                        new Guid(bus.Message.Id),
                        bus.Message.CustomerId,
                        typeof(CampaignOnGoing).FullName,
                        JsonConvert.SerializeObject(bus.Message));

                foreach (var campaignMessage in bus.Message.Campaigns)
                {
                    var campaign = new Campaign(
                        new Guid(campaignMessage.Id),
                        bus.Message.CustomerId,
                        typeof(CampaignOnGoing).FullName,
                        JsonConvert.SerializeObject(campaignMessage));

                    challenge.AddCampaign(campaign);
                }

                await _context.AddAsync(challenge);
                await _context.AddRangeAsync(challenge.Campaigns);
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                throw ex;
            }
        }
    }
}
