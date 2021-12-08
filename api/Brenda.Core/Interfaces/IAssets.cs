using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Core.Interfaces
{
    public interface IAssets : IRepository<Asset>
    {
        Task<IEnumerable<Asset>> GetActiveAsync();
        Task<IEnumerable<Asset>> FindByIdOrName(string search, string actionTag);
        Task<Quiz> GetQuestionByIdAsync(Guid assetId, Guid questionId);
        Task<IEnumerable<Quiz>> GetQuizByIdAsync(Guid id);
        void Remove(Quiz option);
    }
}
