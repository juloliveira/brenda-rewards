using AutoMapper;
using Brenda.Contracts.V1.Requests;
using Brenda.Contracts.V1.Responses;
using Brenda.Core;
using Brenda.Core.Exceptions;
using Brenda.Core.Interfaces;
using Brenda.Core.Services;
using Brenda.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading.Tasks;

namespace Brenda.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CampaignController : BrendaController
    {
        private readonly IUsers _users;
        private readonly ICampaignServices _campaignServices;
        private readonly IPushNotifications _pushNotifications;
        private readonly ICampaigns _campaigns;
        private readonly IMapper _mapper;

        public CampaignController(
            IUsers users,
            ICampaignServices campaignServices,
            IPushNotifications pushNotifications,
            ICampaigns campaigns, 
            IMapper mapper)
        {
            _users = users;
            _campaignServices = campaignServices;
            _campaigns = campaigns;
            _mapper = mapper;
            _pushNotifications = pushNotifications;
        }

        [HttpPost("/campaign/{tag}")]
        public async Task<IActionResult> GetCampaign([FromRoute] string tag, CampaignRequest request)
        {
            if (request.Tag != tag)
                throw new ArgumentException();

            var campaign = await _campaignServices.GetCampaign(request);
            var response = new Response(_mapper.Map<CampaignResponse>(campaign));

            return Ok(response);
        }

        [HttpPut("/campaign/{tag}")]
        public async Task<IActionResult> RewardTicket([FromRoute] string tag, RewardRequest request)
        {
            if (request.Tag != tag)
                throw new ArgumentException();

            (var userRewardTicket, var user) = await _campaignServices.RewardUser(request);
            var response = new Response(_mapper.Map<RewardedTicketResponse>(userRewardTicket));

            await _pushNotifications.NotifyPointsBalance(user.PushToken, user.PointsBalance);

            return Ok(response);
        }

    }
}
