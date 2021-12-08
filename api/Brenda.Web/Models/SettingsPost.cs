using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Brenda.Web.Models
{
    public class SettingsPost
    {
        public IFormFile Logo { get; set; }

        public string Name { get; set; }

        public bool SettingsHasLogo { get; set; }

        public string SettingsLogoAvatar { get; set; }

        public string SettingsDescription { get; set; }

        [EmailAddress(ErrorMessage = "Este e-mail não é válido.")]
        public string SettingsEmail { get; set; }

        public string SettingsPhoneNumber { get; set; }

        [Url(ErrorMessage = "Esta URL não é válida.")]
        public string SettingsSite { get; set; }
    }
}
