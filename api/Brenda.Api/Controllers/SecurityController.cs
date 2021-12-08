using Brenda.Contracts.V1.Requests;
using Brenda.Contracts.V1.Responses;
using Brenda.Core;
using Brenda.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Brenda.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        protected readonly BrendaSettings _settings;
        protected readonly UserManager<BrendaUser> _userManager;

        public SecurityController(
            UserManager<BrendaUser> userManager,
            IOptions<BrendaSettings> options)
        {
            _userManager = userManager;
            _settings = options.Value;
        }

        [HttpGet, Route("test")]
        public IActionResult Teste()
        {
            return Ok(new { Result = "Brenda Rewards" });
        }

        [HttpPost, Route("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserPass model)
        {
            var user = await _userManager.Users
                .Include(x => x.Customer)
                .SingleOrDefaultAsync(x => x.NormalizedUserName == model.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                claims.AddRange(from i in roles
                                select new Claim(ClaimTypes.Role, i));

                if (user.Customer != null)
                    claims.Add(new Claim("customer", user.Customer.Id.ToString()));

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_settings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new AuthToken
                {
                    id = user.Id.ToString(),
                    username = user.Email,
                    token = tokenHandler.WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }


    }
}
