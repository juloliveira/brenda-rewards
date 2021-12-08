using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Brenda.Contracts.V1.Requests
{
    public class AssetForm
    {
        public Guid? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public Guid? ActionId { get; set; }

        public string ActionName { get; set; }

        public bool Enable { get; set; }

        public string Resource { get; set; }
    }
}
