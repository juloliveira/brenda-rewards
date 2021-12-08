using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Contracts.V1.Requests
{
    public class UserPushToken
    {
        public string UserId { get; set; }
        public string PushToken { get; set; }
    }
}
