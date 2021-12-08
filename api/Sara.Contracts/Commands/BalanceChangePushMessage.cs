using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Contracts.Commands
{
    public class BalanceChangePushMessage
    {
        public Guid UserId { get; set; }
        public double Balance { get; set; }
    }
}
