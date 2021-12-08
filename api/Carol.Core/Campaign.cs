using System;
using System.Collections.Generic;

namespace Carol.Core
{
    public class Campaign : Entity
    {
        public List<Transaction> _transactions;

        protected Campaign() 
        {
            _transactions = new List<Transaction>();
        }

        public Campaign(Guid id, string campaignTitle, string customer, double reward, double balance) : base(id)
        {
            Title = campaignTitle;
            Customer = customer;
            Reward = reward;
            Balance = balance;

            _transactions = new List<Transaction>();

            _transactions.Add(new Transaction(
                this,
                title: "Saldo Inicial",
                description: campaignTitle,
                customer: this.Customer,
                operation: Operation.Credit,
                origin: Origin.PublishCampaign,
                reference: id,
                balance));
        }

        public string Title { get; protected set; }

        public string Customer { get; protected set; }

        public double Reward { get; protected set; }

        public double Balance { get; protected set; }


        public IReadOnlyCollection<Transaction> Transactions { get { return _transactions; } }

        public Transaction RewardUser(User user, Guid voucherId, double reward)
        {
            var tx = new Transaction(
                this,
                title: "Recompensa",
                description: "Transaferência para usuário",
                customer: this.Customer,
                operation: Operation.Debt,
                origin: Origin.Voucher,
                reference: voucherId,
                value: reward);
            _transactions.Add(tx);

            Balance -= reward;

            return tx;
        }
    }

}
