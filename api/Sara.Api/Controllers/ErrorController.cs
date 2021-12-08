using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sara.Api.Exceptions;
using Sara.Contracts.Events;

namespace Sara.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IBus _bus;

        public ErrorController(IBus bus)
        {
            _bus = bus;
        }

        [Route("/error")]
        public async Task<IActionResult> ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            await SendExceptionServiceBus(context.Error);

            if (context.Error is IApiException exception)
            {
                return Problem(
                    statusCode: exception.Status,
                    detail: context.Error.StackTrace,
                    title: exception.Message);
            }
            else
            {
                return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
            }
        }

        private async Task SendExceptionServiceBus(Exception error)
        {
            object message = error switch
            {
                CampaignRewardedException ex => new CampaignAlreadyRewarded(ex.CampaignRequest),
                CampaignNotFoundException ex => new CampaignNotFound(ex.CampaignRequest),
                CampaignInvalidLocationException ex => new CampaignInvalidLocation(ex.CampaignRequest),
                CampaignOutOfDateException ex => new CampaignOutOfDate(ex.CampaignRequest),
                _ => null
            };

            if (message != null) await _bus.Publish(message);
        }
    }
}
