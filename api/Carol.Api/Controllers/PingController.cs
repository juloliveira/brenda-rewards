using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Carol.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new { result = "pong" });
        }
    }
}
