using Brenda.Core.Exceptions;
using System;

namespace Brenda.Core
{
    public class AccountActivity : Entity
    {
        protected AccountActivity() { }

        protected internal AccountActivity(
            Customer customer,
            BrendaUser user,
            string description, 
            double value, 
            string reference, 
            string reason)
        {
            if (customer.Id != user.CustomerId)
                throw new ActivityUserInvalidException();

            CustomerId = customer.Id;
            UserId = user.Id;

            Description = description;
            Value = value;
            Reference = reference;
            Reason = reason;
        }

        public string Description { get; protected set; }

        public string Reference { get; set; }

        public string Reason { get; set; }

        public double Value { get; protected set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid UserId { get; protected set; }
        public BrendaUser User { get; protected set; }
    }
}
