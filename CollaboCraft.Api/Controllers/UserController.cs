using CollaboCraft.Models.User;
using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : BaseController
    {
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            return Ok(await userService.GetUser(Id));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return Ok(await userService.GetUser(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await userService.GetUsers());
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe(UpdateUserRequest request)
        {
            request.Id = Id;
            await userService.UpdateUser(request);

            return Ok();
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMe()
        {
            await userService.DeleteUser(Id);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (Role != Role.Admin)
            {
                return Forbid();
            }

            await userService.DeleteUser(id);

            return Ok();
        }

        [HttpPut("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            request.Login = Username;
            await userService.ChangePassword(request);

            return Ok();
        }
    }
}
