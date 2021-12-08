using Brenda.Contracts.V1.Campaign;
using Brenda.Contracts.V1.Customer;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sara.Core;
using Sara.Data;
using System;
using System.Threading.Tasks;

namespace Sara.Api.Consumers
{
    public class CustomerInfoConsumer : IConsumer<CustomerInfo>
    {
        private readonly SaraContext _context;

        public CustomerInfoConsumer(SaraContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CustomerInfo> bus)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(x => x.Id == bus.Message.Id);
            
            if (customer == null)
            {
                customer = new Customer(bus.Message.Id);
                await _context.AddAsync(customer);
            }
            
            customer.Name = bus.Message.Name;
            customer.LogoAvatar = bus.Message.LogoAvatar;
            
            await _context.SaveChangesAsync();
        }
    }
    public class CampaignOnGoingConsumer : IConsumer<CampaignOnGoing>
    {
        private readonly SaraContext _context;

        public CampaignOnGoingConsumer(SaraContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CampaignOnGoing> bus)
        {
            var campaign = new Campaign(
                new Guid(bus.Message.Id),
                bus.Message.CustomerId,
                typeof(CampaignOnGoing).FullName,
                JsonConvert.SerializeObject(bus.Message));

            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
        }
    }
}
