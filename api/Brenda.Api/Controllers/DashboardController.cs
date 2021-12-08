using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brenda.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : BrendaController
    {
        //private readonly IUsers _users;
        //private readonly IDashboard _dashboard;
        //public DashboardController(IDashboard dashboard, IUsers users)
        //{
        //    _users = users;
        //    _dashboard = dashboard;
        //}

        //[HttpGet("activations")]
        //public async Task<IActionResult> GetActivations()
        //{
        //    var user = await _users.GetById(UserId);
        //    if (!user.Customer_Fk.HasValue)
        //        throw new Exception("Operação inválida!");

        //    var activations = await _dashboard.GetActivations(user.Customer_Fk.Value);

        //    return Ok(new { Data = activations });
        //}
    }
}