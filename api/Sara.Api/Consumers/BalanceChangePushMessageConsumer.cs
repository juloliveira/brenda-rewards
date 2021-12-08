using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sara.Api.Infrasctructure;
using Sara.Contracts.Commands;
using Sara.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Api.Consumers
{
    public class BalanceChangePushMessageConsumer : IConsumer<BalanceChangePushMessage>
    {
        private readonly SaraContext _context;
        private readonly IPushMessage _pushMessage;

        public BalanceChangePushMessageConsumer(SaraContext context, IPushMessage pushMessage)
        {
            _context = context;
            _pushMessage = pushMessage;
        }
        public async Task Consume(ConsumeContext<BalanceChangePushMessage> context)
        {
            var token = await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == context.Message.UserId)
                .Select(x => x.FirebaseToken)
                .SingleOrDefaultAsync();

            await _pushMessage.UpdateBalance(context.Message.Balance, token);
        }
    }
}
