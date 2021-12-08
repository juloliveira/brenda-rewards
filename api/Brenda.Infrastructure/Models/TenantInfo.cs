using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Infrastructure.Models
{
    public class TenantInfo
    {
        public Guid? CustomerId { get; set; }
        public Guid? UserId { get; set; }
    }
}
