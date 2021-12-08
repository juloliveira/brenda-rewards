using Brenda.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brenda.Infrastructure.Middleware
{
    public class TenantInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var tenantInfo = context.RequestServices.GetRequiredService<TenantInfo>();

            if (context.User.Identity.IsAuthenticated)
            {
                var companyId = context.User.Claims.FirstOrDefault(x => x.Type == "customer");
                var userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (companyId != null)
                    tenantInfo.CustomerId = System.Guid.Parse(companyId.Value);

                if (userId != null)
                    tenantInfo.UserId = System.Guid.Parse(userId.Value);

            }

            await _next(context);
        }
    }
}
