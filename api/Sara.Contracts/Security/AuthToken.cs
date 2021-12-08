using System;

namespace Sara.Contracts.Security
{
    public struct AuthToken
    {
        public string id { get; set; }
        public string username { get; set; }
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
