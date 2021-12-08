using System;

namespace Carol.Core.Services.Model
{
    public class UserAward
    {
        public UserAward(Guid userId, double reward, double balance, string title, string customer, string firebaseToken)
        {
            UserId = userId;
            Reward = reward;
            Balance = balance;
            CampaignTitle = title;
            CampaignCustomer = customer;
            Token = firebaseToken;
        }

        public Guid UserId { get; set; }
        public double Reward { get; set; }
        public double Balance { get; set; }
        public string CampaignTitle { get; set; }
        public string CampaignCustomer { get; set; }
        public string Token { get; set; }
    }
}
