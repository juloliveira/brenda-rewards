using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Contracts.V1.Responses
{
    public struct AuthToken
    {
        public string id { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
