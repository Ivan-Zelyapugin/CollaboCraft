﻿using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Dapper.Models;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.DataAccess.Repositories.Scripts;

namespace CollaboCraft.DataAccess.Repositories
{
    public class BlockImageRepository(IDapperContext<IDapperSettings> dapperContext) : IBlockImageRepository
    {
        public ITransaction BeginTransaction()
        {
            return dapperContext.BeginTransaction();
        }

        public async Task<int> CreateBlockImage(DbBlockImage blockImage)
        {
            return await dapperContext.CommandWithResponse<int>(new QueryObject(Sql.CreateBlockImage, blockImage));
        }

        public async Task<List<DbBlockImage>> GetImagesByBlockId(int blockId)
        {
            return await dapperContext.ListOrEmpty<DbBlockImage>(new QueryObject(Sql.GetImagesByBlockId, new { blockId }));
        }

        public async Task<DbBlockImage> GetImageById(int id)
        {
            return await dapperContext.FirstOrDefault<DbBlockImage>(new QueryObject(Sql.GetImageById, new { id }));
        }

        public async Task<bool> IsImageExists(int id)
        {
            return await dapperContext.FirstOrDefault<bool>(new QueryObject(Sql.IsImageExists, new { id }));
        }

        public async Task DeleteImage(int id)
        {
            await dapperContext.Command(new QueryObject(Sql.DeleteImage, new { id }));
        }
    }
}
