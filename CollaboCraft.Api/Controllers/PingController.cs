using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    public class PingController : BaseController
    {
        [AllowAnonymous]
        [HttpGet("ping")]
        public Task<IActionResult> Ping()
        {
            return Task.FromResult<IActionResult>(Ok("I'm alive!"));
        }

        [HttpGet("ping-auth")]
        public Task<IActionResult> PingAuth()
        {
            return Task.FromResult<IActionResult>(Ok("I'm alive with auth!"));
        }
    }
}
