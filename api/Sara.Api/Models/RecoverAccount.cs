using System.ComponentModel.DataAnnotations;

namespace Sara.Api.Models
{
    public class RecoverAccount
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Token { get; set; }

        public string CallbackUrl { get; set; }

        
    }
}
