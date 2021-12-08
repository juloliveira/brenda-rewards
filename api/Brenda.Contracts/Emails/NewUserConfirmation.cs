using System;
using System.Collections.Generic;
using System.Text;

namespace Brenda.Contracts.Emails
{
    public class NewUserConfirmation
    {
        public NewUserConfirmation() { }
        public NewUserConfirmation(string token, string email)
        {
            Token = token;
            Email = email;
        }
        public NewUserConfirmation(string name, string email, string callbackUrl)
        {
            Name = name;
            Email = email;
            CallbackUrl = callbackUrl;
        }

        public string Name { get; }
        public string Email { get; set; }
        public string CallbackUrl { get; }
        public string Token { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
