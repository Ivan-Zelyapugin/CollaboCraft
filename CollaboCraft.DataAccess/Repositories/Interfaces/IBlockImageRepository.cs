using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.Models.BlockImage;

namespace CollaboCraft.DataAccess.Repositories.Interfaces
{
    public interface IBlockImageRepository
    {
        ITransaction BeginTransaction();
        Task<int> CreateBlockImage(DbBlockImage blockImage);
        Task<List<DbBlockImage>> GetImagesByBlockId(int blockId);
        Task<DbBlockImage> GetImageById(int id);
        Task<bool> IsImageExists(int id);
        Task DeleteImage(int id);
    }
}
