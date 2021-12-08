using Carol.Core;
using Carol.Data;
using MassTransit;
using Sara.Contracts.Events;
using System.Threading.Tasks;

namespace Carol.Api.Consumers
{
    public class CreateUserConsumer : IConsumer<CreateUser>
    {
        private readonly CarolContext _context;

        public CreateUserConsumer(CarolContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            var user = new User(context.Message.UserId, context.Message.Email, context.Message.PhoneNumber);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
