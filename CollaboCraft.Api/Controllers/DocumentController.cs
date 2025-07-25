﻿using CollaboCraft.Models.Document;
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
            var details = await documentService.GetDocumentDetails(id);

            if (details == null)
                return NotFound();


            var result = new DocumentInfoDto
            {
                Id = details.Id,
                Name = details.Name,
                CreatorUsername = details.Creator?.Username,
                Users = details.Participants
            .Select(p => new DocumentUserDto
            {
                UserId = p.UserId,            
                Username = p.Username,
                Role = p.Role.ToString()
            })
            .ToList()
            };


            return Ok(result);
        }
    }
}
