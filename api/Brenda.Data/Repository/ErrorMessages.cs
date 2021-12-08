using Brenda.Core;
using Brenda.Core.Interfaces;
using Brenda.Core.Validations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Brenda.Data.Repository
{
    public class ErrorMessages : IErrorMessages
    {
        private readonly DbSet<ErrorMessage> _dbSet;
        private readonly BrendaContext _context;

        public ErrorMessages(BrendaContext context)
        {
            _dbSet = context.ErrorMessages;
            _context = context;
        }

        public async Task<CampaignErrorMessage> GetByTagAsync(string tag)
        {
            return await _context.ErrorMessages
                        .Where(x => x.Field == tag)
                        .Select(x => new CampaignErrorMessage(tag, x.FieldName, x.Message))
                        .SingleAsync();
        }
    }
}
