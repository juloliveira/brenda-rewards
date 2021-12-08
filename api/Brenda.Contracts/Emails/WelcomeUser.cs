namespace Brenda.Contracts.Emails
{
    public struct WelcomeUser
    {
        public WelcomeUser(string name, string email, string callbackUrl)
        {
            Name = name;
            Email = email;
            CallbackUrl = callbackUrl;
        }

        public string Name { get; }
        public string Email { get; }
        public string CallbackUrl { get; }
    }
}
