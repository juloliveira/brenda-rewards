using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Brenda.Api.Controllers
{
    public abstract class BrendaController : ControllerBase
    {
        public string UserId => User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        
    }
}
