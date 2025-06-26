using CollaboCraft.Models.Contact;
using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Route("api/[controller]")]
    public class ContactController(IContactService contactService) : BaseController
    {
        [HttpPost("my")]
        public async Task<IActionResult> AddUserToMyContacts(AddUserToContactsRequest request)
        {
            request.UserId = Id;
            await contactService.AddUserToContacts(request);

            return Ok();
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyContacts()
        {
            return Ok(await contactService.GetContactsByUserId(Id));
        }
    }
}
