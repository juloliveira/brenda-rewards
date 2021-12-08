using Brenda.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Brenda.Api.Exceptions
{
    //public class ErrorHandlingMiddleware
    //{
    //    private readonly RequestDelegate next;
    //    private readonly IErrorMessages _errorMessages;

    //    public ErrorHandlingMiddleware(RequestDelegate next, IErrorMessages errorMessages)
    //    {
    //        this.next = next;
    //        _errorMessages = errorMessages;
    //    }

    //    public async Task Invoke(HttpContext context /* other dependencies */)
    //    {
    //        try
    //        {
    //            await next(context);
    //        }
    //        catch (Exception ex)
    //        {
    //            await HandleExceptionAsync(context, ex);
    //        }
    //    }

    //    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    //    {
    //        var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            
    //        var result = JsonConvert.SerializeObject(new { error = ex.Message });
    //        context.Response.ContentType = "application/json";
    //        context.Response.StatusCode = (int)code;
    //        return context.Response.WriteAsync(result);
    //    }
    //}

    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        //private readonly IErrorMessages _errorMessages;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(
            //IErrorMessages errorMessages, 
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            //_errorMessages = errorMessages;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
#pragma warning disable CA1031
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unexpected error");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var json = new
                {
                    context.Response.StatusCode,
                    Message = "An error occurred whilst processing your request",
                    Detailed = exception
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(json));
            }
#pragma warning restore CA1031
        }
    }
}
