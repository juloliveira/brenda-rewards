//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Sara.Api.Exceptions;

//namespace Sara.Api.Filters
//{
//    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
//    {
//        public int Order { get; set; } = int.MaxValue - 10;

//        public void OnActionExecuting(ActionExecutingContext context) { }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            if (context.Exception is IApiException exception)
//            {
//                context.Result = new ObjectResult(exception.Value)
//                {
//                    StatusCode = exception.Status,
//                };

//                context.ExceptionHandled = true;
//            }
//        }
//    }
//}
