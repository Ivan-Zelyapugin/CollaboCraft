﻿using CollaboCraft.Models.Block;

namespace CollaboCraft.Services.Interfaces
{
    public interface IBlockService
    {
        Task<Block> SendBlock(SendBlockRequest request);
        Task<List<Block>> GetBlocksByDocument(int userId, int documentId, DateTime from);
        Task<Block> EditBlock(EditBlockRequest request);
    }
}
