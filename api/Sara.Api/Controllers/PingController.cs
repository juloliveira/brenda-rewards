using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sara.Contracts.Commands;
using Sara.Data;
using Sara.Infrastructure.Security;
using System.Linq;
using System.Threading.Tasks;

namespace Sara.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Result = "pong", Auth = User.Identity.IsAuthenticated }); ;
        }

#if DEBUG
        [HttpPost]
        public async Task<IActionResult> Post([FromServices] SaraContext context, [FromServices] IBus bus)
        {
            //Gender
            foreach (var item in await context.GenderIdentities.ToListAsync())
                await bus.Publish(new Descriptor { Id = item.Id.ToString(), Description = item.Description, Entity = item.GetType().FullName });

            //Sexuality
            foreach (var item in await context.Sexualities.ToListAsync())
                await bus.Publish(new Descriptor { Id = item.Id.ToString(), Description = item.Description, Entity = item.GetType().FullName });

            //Income
            foreach (var item in await context.Incomes.ToListAsync())
                await bus.Publish(new Descriptor { Id = item.Id.ToString(), Description = item.Description, Entity = item.GetType().FullName });

            //Educational Levels
            foreach (var item in await context.EducationLevels.ToListAsync())
                await bus.Publish(new Descriptor { Id = item.Id.ToString(), Description = item.Description, Entity = item.GetType().FullName });

            return Ok("Done!");
        }
#endif
    }
}
