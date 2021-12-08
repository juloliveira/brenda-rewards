using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brenda.Core.Validations
{
    public class PublishValidation 
    {
        private readonly IList<CampaignErrorMessage> _errorMessages;

        public PublishValidation()
        {
            _errorMessages = new List<CampaignErrorMessage>();
        }

        public virtual int Errors => _errorMessages.Count;

        public virtual int TotalValidations { get; private set; }

        public virtual int Complete => (int)((1 - (_errorMessages.Count / ((double)TotalValidations))) * 100);

        public virtual IEnumerable<CampaignErrorMessage> ErrorMessages => _errorMessages;

        public virtual void AddErrorMessage(CampaignErrorMessage errorMessage)
        {
            _errorMessages.Add(errorMessage);
        }

        public virtual void AddValidation()
        {
            TotalValidations++;
        }

        public virtual bool IsValid => !_errorMessages.Any();
    }
}
