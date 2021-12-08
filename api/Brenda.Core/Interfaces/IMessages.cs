using Brenda.Core.Validations;
using System.Threading.Tasks;

namespace Brenda.Core.Interfaces
{
    public interface IErrorMessages
    {
        Task<CampaignErrorMessage> GetByTagAsync(string tag);
    }
}
