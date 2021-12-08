using System.ComponentModel.DataAnnotations;

namespace Brenda.Web.Models
{
    public class SigninPost
    {
        [Required, EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}