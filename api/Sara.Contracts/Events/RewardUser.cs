using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Contracts.Events
{
    public class RewardUser
    {
        public RewardUser(Guid userId, Guid voucherId, double reward)
        {
            UserId = userId;
            VoucherId = voucherId;
            Reward = reward;
        }

        public Guid UserId { get; }
        public Guid VoucherId { get; }
        public double Reward { get; }
    }
}
