using Brenda.Core.Validations;
using System.Threading.Tasks;

namespace Brenda.Core.Services
{
    public interface ICampaignValidator
    {
        Task<PublishValidation> ValidateAsync(Campaign campaign);
    }
}
