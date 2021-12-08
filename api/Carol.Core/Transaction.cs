using System;

namespace Carol.Core
{
    public class Transaction : Entity
    {
        protected Transaction() { }

        public Transaction(
            User user,
            string title,
            string description,
            string customer,
            Operation operation,
            Origin origin,
            Guid reference,
            double value) : this(title, description, customer, operation, origin, reference, value)
        {
            UserId = user.Id;
        }

        public Transaction(
            Campaign campaign,
            string title,
            string description,
            string customer,
            Operation operation,
            Origin origin,
            Guid reference,
            double value) : this(title, description, customer, operation, origin, reference, value)
        {
            CampaignId = campaign.Id;
        }

        private Transaction(
            string title,
            string description,
            string customer,
            Operation operation,
            Origin origin,
            Guid reference,
            double value)
        {
            Title = title;
            Description = description;
            Customer = customer;
            Operation = operation;
            Origin = origin;
            Reference = reference;
            Value = value;
        }

        public string Title { get; protected set; }

        public string Description { get; protected set; }

        public string Customer { get; protected set; }

        public Origin Origin { get; protected set; }

        public Guid Reference { get; protected set; }

        public Operation Operation { get; protected set; }

        public double Value { get; protected set; }

        public Guid? UserId { get; set; }
        public User User { get; set; }

        public Guid? CampaignId { get; set; }
        public Campaign Campaign { get; set; }

    }

}
