using System.Collections.Generic;

namespace Brenda.Core
{
    public class Customer : Entity
    {
        private List<BrendaUser> _users = new List<BrendaUser>();
        protected readonly List<AccountActivity> _accountStatement;

        protected Customer() { }
        public Customer(string name, string document)
        {
            _accountStatement = new List<AccountActivity>();
            Settings = new Settings();

            Name = name;
            Document = document;
        }

        public string Name { get; set; }
        public string Document { get; set; }

        public double Balance { get; protected set; }

        public Settings Settings { get; protected set; }

        public ISet<Campaign> Campaigns { get; set; }

        public IReadOnlyCollection<AccountActivity> AccountStatement => _accountStatement.AsReadOnly();

        public IReadOnlyCollection<BrendaUser> Users { get { return _users; } }

        public void AddUser(BrendaUser user)
        {
            user.SetCustomer(this);
            _users.Add(user);
        }

        public AccountActivity Transfer(BrendaUser user, Campaign to, double value)
        {
            this.Balance -= value;
            to.AddBalance(value);
            var accountActivity = new AccountActivity(
                this,
                user,
                "Ordem de Transferência", 
                (value * (-1)), 
                to.Title, 
                "Transferência");

            // TODO: EF Core é bem ruim!
            if (_accountStatement != null) _accountStatement.Add(accountActivity);

            return accountActivity;
        }

        public Campaign AddCampaign()
        {
            return new Campaign(this);
        }
    }
}
