using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Brenda.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sara.Api.Models;
using Sara.Contracts.Events;
using Sara.Contracts.Security;
using Sara.Core;
using Sara.Core.Factories;
using Sara.Infrastructure.Security;

namespace Sara.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<SaraUser> _userManager;
        private readonly IUserFactory _userFactory;
        private readonly IEmailSender _emailSender;
        private readonly IEmailRenderer _emailRenderer;
        private readonly SaraSettings _settings;
        private readonly IBus _bus;

        public SecurityController(
            IOptions<SaraSettings> options,
            UserManager<SaraUser> userManager,
            IUserFactory userFactory,
            IEmailSender emailSender,
            IEmailRenderer emailRenderer,
            IBus bus)
        {
            _userManager = userManager;
            _userFactory = userFactory;
            _emailSender = emailSender;
            _emailRenderer = emailRenderer;
            _settings = options.Value;
            _bus = bus;
        }

        [HttpPost, Route("recover")]
        public async Task<IActionResult> Recover(RecoverAccount recover)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(recover.Email);
                if (user == null) throw new ArgumentNullException(nameof(recover));
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));
                recover.CallbackUrl = $"https:////192.168.15.18:8421/user/reset?id={user.Id}&token={token}";

                var emailRendered = await _emailRenderer.RenderHtmlAsync(recover);
                await _emailSender.SendEmail(null, user.Email, emailRendered);

                return Ok();
            }

            throw new ArgumentException(nameof(recover));
        }



        [HttpPost, Route("create")]
        public async Task<IActionResult> Create(UserRegister userRegister)
        {
            var recaptcha = await Brenda.Utils.ReCaptcha.Validate("6Lc_rb0ZAAAAAPntm7BF7QRqO65Tm4PjFOVQ3LP0", userRegister.Token);

            if (!recaptcha.Success)
                throw new ArgumentException("Invalid Token!");

            var newUser = await _userFactory.Create(userRegister);
            newUser.EmailConfirmed = true;
            userRegister.Password = GeneratePassword();

            var result = await _userManager.CreateAsync(newUser, userRegister.Password);
            await _userManager.SetLockoutEnabledAsync(newUser, false);
            await _userManager.AddToRoleAsync(newUser, "Default");
            if (!result.Succeeded)
                return Problem(result.Errors.ElementAt(0).Description);

            var email = await _emailRenderer.RenderHtmlAsync(userRegister);
            await _emailSender.SendEmail(null, userRegister.Email, email);

            await _bus.Publish(
                new CreateUser
                {
                    UserId = newUser.Id,
                    Email = newUser.Email,
                    PhoneNumber = newUser.PhoneNumber,
                    Birthdate = newUser.Birthdate,
                    Sex = (int)newUser.Sex,
                    GenderIdentityId = newUser.GenderIdentityId,
                    IncomeId = newUser.IncomeId,
                    SexualityId = newUser.SexualityId,
                    EducationLevelId = newUser.EducationLevelId,
                    Latitude = userRegister.Latitude,
                    Longitude = userRegister.Longitude,
                    DeviceId = userRegister.DeviceId,
                    DeviceData = userRegister.DeviceData,
                    CreatedAt = DateTime.UtcNow
                });

            return Ok(new { result = true });
        }

        [AllowAnonymous, HttpPost, Route("sign-in")]
        public async Task<IActionResult> SignIn(
            [FromBody] AccessCredentials credenciais,
            [FromServices] AccessManager accessManager)
        {
            if (await accessManager.ValidateCredentials(credenciais))
            {
                return Ok(await accessManager.GenerateToken(credenciais));
            }
            else
            {
                return Unauthorized(new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar"
                });
            }
        }

        [HttpPut("/token"), Authorize]
        public async Task<IActionResult> PutFirebaseToken(FirebaseToken firebaseToken)
        {
            if (firebaseToken == null) throw new ArgumentNullException(nameof(firebaseToken));

            var user = await _userManager.GetUserAsync(this.User);
            await _bus.Publish(new Carol.Contracts.FirebaseUserTokenUpdate(user.Id, firebaseToken.Token));

            user.FirebaseToken = firebaseToken.Token;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        public string GeneratePassword()
        {
            const string allowChars = "Aa0BbC@c1D#dEe2F$fGg3Hh4%IiJj5Kk&LlM6m&NnOo7Pp%QqRr8SsTt$Uu9V#vWwXxYy@Zz";
            var password = new StringBuilder();
            var random = new Random();

            bool digit = _userManager.Options.Password.RequireDigit;
            bool lowercase = _userManager.Options.Password.RequireLowercase;
            bool uppercase = _userManager.Options.Password.RequireUppercase;

            while (password.Length < _userManager.Options.Password.RequiredLength)
            {
                var @char = allowChars.ElementAt(random.Next(0, allowChars.Length - 1));
                password.Append(@char);

                if (char.IsDigit(@char))
                    digit = false;
                else if (char.IsLower(@char))
                    lowercase = false;
                else if (char.IsUpper(@char))
                    uppercase = false;
            }

            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            return password.ToString();
        }
    }


    public class FirebaseToken
    {
        public string Token { get; set; }
    }


}
