using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlockController(IBlockService blockService) : BaseController
    {
        [HttpGet("{documentId:int}")]
        public async Task<IActionResult> GetBlocksByDocument(int documentId, [FromQuery] DateTime from)
        {
            return Ok(await blockService.GetBlocksByDocument(Id, documentId, from));
        }
    }
}
