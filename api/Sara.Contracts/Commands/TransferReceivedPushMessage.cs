using System;

namespace Sara.Contracts.Commands
{
    public class TransferReceivedPushMessage
    {
        public Guid UserId { get; set; }
        public string FromUserEmail { get; set; }
        public double Value { get; set; }
        public double Balance { get; set; }
    }
}
