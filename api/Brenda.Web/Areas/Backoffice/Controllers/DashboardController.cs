using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Web.Areas.Rock.Controllers
{
    [Area("Backoffice")]
    [Authorize(Brenda.Core.Roles.ROOT)]
    public class DashboardController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
