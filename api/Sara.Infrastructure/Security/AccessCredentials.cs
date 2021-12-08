using Sara.Core;
using System;

namespace Sara.Infrastructure.Security
{
    public class RefreshTokenData
    {
        protected RefreshTokenData() { }
        public RefreshTokenData(Guid userId, string username, string refreshToken) 
        {
            UserId = userId;
            Username = username;
            RefreshToken = refreshToken;
        }

        public Guid UserId { get; protected set; }

        public string Username { get; protected set; }

        public string RefreshToken { get; protected set; }
    }

    public class AccessCredentials
    {
        private string _username;

        public string Username 
        { 
            get { return _username?.Trim(); }
            set { _username = value; }
        }

        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
        public SaraUser User { get; internal set; }
        public Guid UserId { get; internal set; }
    }

    public class Token
    {
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public Guid UserId { get; set; }
    }
}
