using CollaboCraft.Common;
using CollaboCraft.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private string AuthHeader => HttpContext.Request.Headers.Authorization.ToString();

        protected int Id => int.Parse(Jwt.GetId(AuthHeader));
        protected Role Role => Enum.Parse<Role>(Jwt.GetRole(AuthHeader));
        protected string Email => Jwt.GetEmail(AuthHeader);
        protected string Username => Jwt.GetUsername(AuthHeader);
    }
}
