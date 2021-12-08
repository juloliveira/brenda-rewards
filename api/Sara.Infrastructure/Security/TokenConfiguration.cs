using System;
using System.Collections.Generic;
using System.Text;

namespace Sara.Infrastructure.Security
{
    public class TokenConfiguration
    {
        public string ReferralUrl { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string ReferralId { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public int AccessTokenLifetimeSeconds { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }
    }
}
