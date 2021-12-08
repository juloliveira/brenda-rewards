using System;
using System.Collections.Generic;
using System.Text;

namespace Carol.Contracts
{
    public class FirebaseUserTokenUpdate
    {
        protected FirebaseUserTokenUpdate() { }

        public FirebaseUserTokenUpdate(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public Guid UserId { get; set; }

        public string Token { get; set; }
    }
}
