using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Brenda.Contracts.V1.Requests;
using Brenda.Contracts.V1.Responses;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : BrendaController
    {
        private readonly IUsers _users;
        private readonly IMapper _mapper;
        private readonly IUserRegisterService _userRegisterService;

        public UserController(IUsers users, IMapper mapper, IUserRegisterService userRegisterService)
        {
            _users = users;
            _mapper = mapper;
            _userRegisterService = userRegisterService;

        }

        [HttpGet("/user/confirm-email"), AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            await _userRegisterService.ConfirmEmail(token, email);

            return Ok(Responses.Successful);
        }

        [HttpPost("/user/register"), AllowAnonymous]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (ModelState.IsValid)
            {
                if (!await _userRegisterService.Register(userRegister))
                    return BadRequest();
                
                return Ok(Responses.Successful);
            }

            throw new Exception("Requisição inválida");
        }

        [HttpGet("/user")]
        public async Task<IActionResult> GetDashboard()
        {
            var user = await _users.GetCurrentAsync();
            var response = new Response(_mapper.Map<UserDashboard>(user));

            return Ok(response);
        }

        [HttpPut("/user/token")]
        public async Task<IActionResult> UpdateUserToken(UserPushToken request)
        {
            var user = await _users.GetCurrentAsync();
            user.PushToken = request.PushToken;

            await _users.SaveChanges();

            return Ok(new { result = "success" });
        }
    }
}