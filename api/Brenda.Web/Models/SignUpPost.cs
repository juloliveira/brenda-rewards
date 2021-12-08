namespace Brenda.Web.Models
{
    public class SignUpPost
    {
        public string CompanyName { get; set; }
        public string CompanyDocument { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}