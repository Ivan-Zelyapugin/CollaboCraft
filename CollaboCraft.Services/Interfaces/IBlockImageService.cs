using CollaboCraft.Models.BlockImage;

namespace CollaboCraft.Services.Interfaces
{
    public interface IBlockImageService
    {
        Task<BlockImage> SendBlockImage(SendBlockImageRequest request, FileUpload file);
        Task<List<BlockImage>> GetBlockImagesByBlock(int userId, int blockId);
        Task DeleteBlockImage(int id, int userId);
    }
}
