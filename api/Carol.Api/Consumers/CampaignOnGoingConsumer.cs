using Brenda.Contracts.V1.Campaign;
using Carol.Core;
using Carol.Data;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Carol.Api.Consumers
{
    public class CampaignOnGoingConsumer : IConsumer<CampaignOnGoing>
    {
        private readonly CarolContext _context;

        public CampaignOnGoingConsumer(CarolContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CampaignOnGoing> bus)
        {
            var campaign = new Campaign(
                new Guid(bus.Message.Id), 
                bus.Message.Title, 
                bus.Message.CustomerName, 
                bus.Message.Reward, 
                bus.Message.Balance);

            await _context.AddAsync(campaign);
            await _context.AddAsync(campaign.Transactions.First());
            await _context.SaveChangesAsync();
        }
    }
}
