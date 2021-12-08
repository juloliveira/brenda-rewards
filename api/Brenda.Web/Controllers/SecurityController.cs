using System.Security.Claims;
using System.Threading.Tasks;
using Brenda.Contracts.Emails;
using Brenda.Core;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Brenda.Data;
using Brenda.Utils;
using Brenda.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Brenda.Web.Controllers
{
    public class SecurityController : Controller
    {

        protected readonly BrendaSettings _settings;
        protected readonly UserManager<BrendaUser> _userManager;

        private readonly IUserRegisterService _userRegistry;

        public SecurityController(
            UserManager<BrendaUser> userManager,
            IOptions<BrendaSettings> options,
            IUserRegisterService userRegistry)
        {
            _userManager = userManager;
            _settings = options.Value;
            _userRegistry = userRegistry;
        }

        [HttpGet("security/sign-in")]
        public IActionResult Signin(string returnUrl)
        {
            if (User != null && User.Identity.IsAuthenticated)
                return Redirect("/");

            ViewBag.ReturnUrl = returnUrl;

            return View(new SigninPost());
        }

        [ValidateAntiForgeryToken]
        [HttpPost("security/sign-in")]
        public async Task<IActionResult> SignIn(SigninPost model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.Users
                    .Include(x => x.Customer)
                    .SingleOrDefaultAsync(x => x.NormalizedUserName == model.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
                    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                    foreach (var role in roles)
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));

                    
                    identity.AddClaim(new Claim("customer", user.CustomerId.ToString()));
                    identity.AddClaim(new Claim("customer_name", user.Customer.Name));

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return Redirect(model.ReturnUrl ?? "/");
                }
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost("security/sign-up")]
        public async Task<IActionResult> SignUp(SignUpPost post,
            [FromServices] IUnitOfWork uow)
        {
            if (ModelState.IsValid)
            {
                var recaptcha = await Brenda.Utils.ReCaptcha.Validate("6LctLMsZAAAAAL5Hl3ZwrfTrvysqI4HgRYfDOpFV", post.Token);

                if (!recaptcha.Success)
                    throw new System.ArgumentException();

                if (await uow.Customers.HasDocumentAsync(post.CompanyDocument.ClearDocumentChars()))
                {
                    ModelState.AddModelError("", "CNPJ já cadastrado");
                }
                else
                {
                    await _userRegistry.Register(post.CompanyName, post.CompanyDocument, post.Name, post.Email, post.Password, Roles.Administrator);

                    return Ok(Brenda.Contracts.V1.Responses.Responses.Successful);
                }
            }

            System.Collections.Generic.List<string> errors = new System.Collections.Generic.List<string>();
            foreach (var modelState in ViewData.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }

            return Ok(Brenda.Contracts.V1.Responses.Responses.Errors(errors));
        }

        [HttpGet("security/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            await _userRegistry.ConfirmEmail(token, email);

            return View();
        }

        [HttpGet("security/new-user-confirmation")]
        public async Task<IActionResult> NewUserConfirmation(string token, string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            var validToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token.Base64ForUrlDecode());

            if (!validToken)
                return Redirect("https://brendarewards.com");

            return View(new NewUserConfirmation(token, email));
        }

        [HttpPost("security/new-user-confirmation")]
        public async Task<IActionResult> NewUserConfirmation(NewUserConfirmation post)
        {
            var user = await _userManager.FindByNameAsync(post.Email);
            if (user == null) return Redirect("https://brendarewards.com");
            var validToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", post.Token.Base64ForUrlDecode());
            if (!validToken) return Redirect("https://brendarewards.com");

            if (ModelState.IsValid)
            {
                user.EmailConfirmed = true;
                var result = await _userManager.ResetPasswordAsync(user, post.Token.Base64ForUrlDecode(), post.Password);
                if (result.Succeeded)
                    return RedirectToAction("Signin", "Security");
            }

            return View(post);
        }

        [HttpGet("security/access-denied")]
        public IActionResult Denied() => View();
    }

    
}