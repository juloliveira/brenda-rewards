using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Sara.Api.Models;
using Sara.Core;

namespace Sara.Api.Controllers
{
    public partial class UserController : Controller
    {

        [HttpGet, Route("user/reset")]
        public async Task<IActionResult> ResetPassword(Guid id, string token,
            [FromServices] UserManager<SaraUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return View(new ResetForm(id, token));
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var tokenString = Encoding.UTF8.GetString(tokenBytes);
            var validToken = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", tokenString);
            if (!validToken) return Redirect("https://brendarewards.com");

            return View(new ResetForm(id, token));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Route("user/reset")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> ResetPassword(
            [FromForm] ResetForm reset,
            [FromServices] UserManager<SaraUser> userManager)
        {
            var user = await userManager.FindByIdAsync(reset.UserId.ToString());
            if (user == null)
            {
                TempData["Action"] = "fake";
                return RedirectToAction("confirm");
            }

            var tokenBytes = WebEncoders.Base64UrlDecode(reset.Token);
            var tokenString = Encoding.UTF8.GetString(tokenBytes);

            var validToken = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", tokenString);
            if (!validToken)
            {
                TempData["Action"] = "fake";
                return RedirectToAction("confirm");
            }

            var result = await userManager.ResetPasswordAsync(user, tokenString, reset.Password);
            if (result.Succeeded)
            {
                TempData["Action"] = reset.UserId;
                return RedirectToAction("confirm");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(new ResetForm(reset.UserId, reset.Token));
        }

        [HttpGet]
        [Route("user/confirm")]
        public IActionResult Confirm()
        {
            if (TempData["Action"] == null) return Redirect("https://brendarewards.com");
            if (TempData["Action"].ToString() == "fake")
            {
                ModelState.AddModelError("", "Ocorreu algum problema ao alterar sua senha.");
                return View("ResetPassword", new ResetForm(Guid.NewGuid(), Guid.NewGuid().ToString()));
            }

            return View();
        }
    }
}
