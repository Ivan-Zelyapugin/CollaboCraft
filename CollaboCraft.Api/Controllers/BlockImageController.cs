using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollaboCraft.Api.Controllers
{
    [Route("api/[controller]")]
    public class BlockImageController(IBlockImageService blockImageService) : BaseController
    {
        [HttpGet("blocks/{blockId}/images")]
        public async Task<IActionResult> GetBlockImages(int blockId)
        {
            return Ok(await blockImageService.GetBlockImagesByBlock(Id, blockId));
        }
    }
}
