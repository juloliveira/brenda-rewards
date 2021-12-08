using System;

namespace Sara.Api.Models
{
    public class ResetForm
    {
        public ResetForm() { }
        public ResetForm(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }

        public Guid UserId { get; set; }
        public string Token { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
