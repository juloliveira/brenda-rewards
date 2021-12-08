using System;
using System.Collections.Generic;
using System.Text;

namespace Carol.Core
{
    public class User : Entity
    {
        public List<Transaction> _transactions;

        protected User() 
        {
            _transactions = new List<Transaction>();
        }

        public User(Guid id, string email, string phoneNumber) : base(id) 
        {
            Balance = 0;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public double Balance { get; set; }

        public IReadOnlyCollection<Transaction> Transactions { get { return _transactions; } }

        public string FirebaseToken { get; set; }
        public string Email { get; set; }

        public Transaction Transfer(User to, double value)
        {
            var tx = new Transaction(
                this,
                title: "Transferência",
                description: $"Para: {to.Email}",
                customer: null,
                operation: Operation.Debt,
                origin: Origin.TransferToUser,
                reference: to.Id,
                value: value);
            _transactions.Add(tx);
            this.Balance -= value;

            return tx;
        }

        public object ReceiveTransfer(User sender, Transaction txSender)
        {
            var tx = new Transaction(
                this,
                title: "Transferência Recebida",
                description: $"De: {sender.Email}",
                customer: null,
                operation: Operation.Credit,
                origin: Origin.TransferReceived,
                reference: txSender.UserId.Value,
                value: txSender.Value);
            _transactions.Add(tx);
            this.Balance += txSender.Value;

            return tx;
        }

        public string PhoneNumber { get; set; }

        public object ReceiveReward(Campaign campaign, Transaction campaignTransaction, string customerName)
        {
            var tx = new Transaction(
                this,
                title: "Recompensa Recebida",
                description: campaign.Title,
                customer: customerName,
                operation: Operation.Credit,
                origin: campaignTransaction.Origin,
                reference: campaignTransaction.Reference,
                value: campaignTransaction.Value
                );
            _transactions.Add(tx);

            this.Balance += campaignTransaction.Value;

            return tx;
        }
    }
}
