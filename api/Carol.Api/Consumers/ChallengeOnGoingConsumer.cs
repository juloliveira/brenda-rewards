using Brenda.Contracts.V1.Campaign;
using Carol.Core;
using Carol.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Carol.Api.Consumers
{
    public class ChallengeOnGoingConsumer : IConsumer<ChallengeOnGoing>
    {
        private readonly CarolContext _context;

        public ChallengeOnGoingConsumer(CarolContext context)
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
                    bus.Message.Title,
                    bus.Message.CustomerName,
                    bus.Message.Reward,
                    bus.Message.Balance);

                await _context.AddAsync(challenge);
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
