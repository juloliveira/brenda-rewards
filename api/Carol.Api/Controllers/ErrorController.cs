using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Carol.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult ErrorLocalDevelopment(
        [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error is Exception exception)
            {
                return Problem(
                    //statusCode: exception.Status,
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
