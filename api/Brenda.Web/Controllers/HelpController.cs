using Microsoft.AspNetCore.Mvc;

namespace Brenda.Web.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
