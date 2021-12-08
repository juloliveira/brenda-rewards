using System;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Web.Controllers
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
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            
            if (context.Error is Exception exception)
            {
                return Problem(
                    statusCode: 403,
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

    }
}
