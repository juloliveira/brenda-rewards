using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Brenda.Contracts.V1.Requests
{
    public class ResourcePut
    {
        [Required, Url]
        public string Resource { get; set; }
    }
}
