using Carol.Contracts;
using Carol.Core;
using Carol.Data;
using MassTransit;
using System.Threading.Tasks;

namespace Carol.Api.Consumers
{
    public class FirebaseUserTokenUpdateConsumer : IConsumer<FirebaseUserTokenUpdate>
    {
        private readonly CarolContext _context;

        public FirebaseUserTokenUpdateConsumer(CarolContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<FirebaseUserTokenUpdate> context)
        {
            var user = new User(context.Message.UserId, null, null) { FirebaseToken = context.Message.Token };
            _context.Attach(user);
            _context.Entry(user).Property(x => x.FirebaseToken).IsModified = true;
            await _context.SaveChangesAsync();
        }
    }
}
