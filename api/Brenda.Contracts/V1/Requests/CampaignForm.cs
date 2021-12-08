using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Brenda.Contracts.V1.Requests
{
    public class CampaignForm
    {
        public Guid? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid ActionId { get; set; }

        [Required]
        public double? Reward { get; set; }

        private DateTime? _validationStart;
        private DateTime? _validationEnd;
        [Required]
        public DateTime? DefinitionsValidationStart
        {
            get { return _validationStart; }
            set
            {
                if (value.HasValue)
                    _validationStart = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 0, 0, 0);
            }
        }

        [Required]
        public DateTime? DefinitionsValidationEnd
        {
            get { return _validationEnd; }
            set
            {
                if (value.HasValue)
                    _validationEnd = new DateTime(value.Value.Year, value.Value.Month, value.Value.Day, 23, 59, 59);
            }
        }

        public bool DefinitionsValidateGeoLocation { get; set; }

        
    }
}
