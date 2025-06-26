using CollaboCraft.Services;
using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController(IDocumentService documentService) : BaseController
    {

        [HttpGet("my")]
        public async Task<IActionResult> GetMyDocuments()
        {
            return Ok(await documentService.GetDocumentsByUserId(Id));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDocumentDetails(int id)
        {
            return Ok(await documentService.GetDocumentDetails(id));
        }
    }
}
