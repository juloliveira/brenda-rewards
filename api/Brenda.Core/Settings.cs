using System;

namespace Brenda.Core
{
    public class Settings : Entity
    {
        public string Description { get; set; }

        public bool HasLogo { get; protected set; }
        public string LogoOriginal { get; protected set; }
        public string LogoAvatar { get; protected set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Site { get; set; }


        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }


        

        public void SetLogo(string originalPath, string avatarPath)
        {
            HasLogo = true;
            LogoOriginal = originalPath;
            LogoAvatar = avatarPath;
        }
    }
}
