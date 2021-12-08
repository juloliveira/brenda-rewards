using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Core.DTO
{
    public class AccountOverview
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }

        public IEnumerable<AccountActivity> AccountStatement { get; set; }
        public bool HasLogo { get; set; }
        public string LogoAvatar { get; set; }
        public string Email { get; set; }

        public class AccountActivity
        {
            public Guid Id { get; set; }

            public string Description { get; set; }

            public string Reference { get; set; }

            public string Reason { get; set; }

            public double Value { get; set; }

            public DateTime CreatedAt { get; set; }
            
        }
    }
}
