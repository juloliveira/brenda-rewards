using Brenda.Core.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Core.Services
{
    public interface ICampaignPublisher
    {
        Task<PublishValidation> Publish(Campaign campaign);
    }
}
