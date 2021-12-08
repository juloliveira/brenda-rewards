using System.ComponentModel.DataAnnotations;

namespace Brenda.Web.Models
{
    public class UserForm
    {
        [Required]
        public string Name { get; set; }


        [Required]
        [EmailAddress()]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
