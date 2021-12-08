using MassTransit;
using Microsoft.EntityFrameworkCore;
using Sara.Api.Infrasctructure;
using Sara.Contracts.Commands;
using Sara.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Api.Consumers
{
    public class TransferReceivedPushMessageConsumer : IConsumer<TransferReceivedPushMessage>
    {
        private readonly SaraContext _context;
        private readonly IPushMessage _pushMessage;

        public TransferReceivedPushMessageConsumer(SaraContext context, IPushMessage pushMessage)
        {
            _context = context;
            _pushMessage = pushMessage;
        }

        public async Task Consume(ConsumeContext<TransferReceivedPushMessage> context)
        {
            var token = await _context.Users
                .AsNoTracking()
                .Where(x => x.Id == context.Message.UserId)
                .Select(x => x.FirebaseToken)
                .SingleOrDefaultAsync();

            await _pushMessage.TransferReceivedMessage(context.Message, token);
        }
    }
}
