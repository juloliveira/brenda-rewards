using Microsoft.AspNetCore.Mvc;

namespace Julia.Api.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Result = "Pong" });
        }
    }
}

