using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brenda.Core;
using Brenda.Core.Exceptions;
using Brenda.Core.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Brenda.Api.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [Route("/error-local-development")]
        public  IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context.Error is BrendaException)
            {
                var error = context.Error as BrendaException;
            }

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}