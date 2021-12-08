using System.Linq;
using System.Threading.Tasks;
using Brenda.Contracts.V1.Responses;
using Brenda.Core;
using Brenda.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Brenda.Web.Controllers
{
    public abstract class BrendaController : Controller
    {
        protected async Task LoadActions(IUnitOfWork uow)
        {
            var formatsList = await uow.Actions.GetBaseAllAsync();
            ViewBag.Formats = formatsList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
        }

        protected async Task LoadCampaigns(IUnitOfWork uow, CampaignStatus status)
        {
            var campaignsList = await uow.Campaigns.GetByStatusAsync(status);
            ViewBag.Campaigns = campaignsList.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
        }

        public RedirectToActionResult Error(string actionName, object routeValues, string message)
        {
            TempData["Error"] = message;
            return RedirectToAction(actionName, routeValues);
        }

        public IActionResult RedirectJson(string redirect) =>
            Json(Responses.Redirect(redirect));


    }

}
