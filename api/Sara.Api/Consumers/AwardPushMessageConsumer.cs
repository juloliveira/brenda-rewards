using MassTransit;
using Sara.Api.Infrasctructure;
using Sara.Contracts.Commands;
using Sara.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Api.Consumers
{
    public class AwardPushMessageConsumer : IConsumer<AwardPushMessage>
    {
        private readonly SaraContext _context;
        private readonly IPushMessage _pushMessage;

        public AwardPushMessageConsumer(SaraContext context, IPushMessage pushMessage)
        {
            _context = context;
            _pushMessage = pushMessage;
        }
        
        public async Task Consume(ConsumeContext<AwardPushMessage> context)
        {
            var token = _context.Users
                .Where(x => x.Id == context.Message.UserId)
                .Select(x => x.FirebaseToken)
                .SingleOrDefault();

            await _pushMessage.AwardMessage(context.Message, token);
        }
    }
}
