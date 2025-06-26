using CollaboCraft.Models.Block;
using CollaboCraft.Models.Document;
using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CollaboCraft.Api.Hubs
{
    public class DocumentHub(IConnectionTracker connectionTracker, IDocumentService documentService, IBlockService messageService, IDocumentParticipantService documentParticipantService) : BaseHub
    {
        public async Task CreateDocument(CreateDocumentRequest request)
        {
            try
            {
                request.CreatorId = Id;
                var document = await documentService.CreateDocument(request);

                var connectionIds = connectionTracker.SelectConnectionIds(request.UserIds).Append(Context.ConnectionId);
                await Task.WhenAll(connectionIds.Select(connectionId => Groups.AddToGroupAsync(connectionId, $"Document{document.Id}")));

                await Clients.Caller.SendAsync("DocumentCreated", document);
                await Clients.Group($"Document{document.Id}").SendAsync("AddedToDocument", request.UserIds);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task SendBlock(SendBlockRequest request)
        {
            try
            {
                request.UserId = Id;
                var message = await messageService.SendBlock(request);
                await Clients.Group($"Document{message.DocumentId}").SendAsync("ReceiveBlock", message);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task EditBlock(EditBlockRequest request)
        {
            try
            {
                request.UserId = Id;
                var editedBlock = await messageService.EditBlock(request);
                await Clients.Group($"Document{editedBlock.DocumentId}").SendAsync("BlockEdited", editedBlock);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task AddUsersToDocument(AddUsersToDocumentRequest request)
        {
            try
            {
                request.RequestingUserId = Id;
                await documentService.AddUsersToDocument(request);

                var connectionIds = connectionTracker.SelectConnectionIds(request.UserIds);
                await Task.WhenAll(connectionIds.Select(connectionId => Groups.AddToGroupAsync(connectionId, $"Document{request.DocumentId}")));
                await Clients.Group($"Document{request.DocumentId}").SendAsync("AddedToDocument", request.UserIds);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public override async Task OnConnectedAsync()
        {
            connectionTracker.TrackConnection(Context.ConnectionId, Id);

            var documentParticipants = await documentParticipantService.GetDocumentParticipantsByUserId(Id);
            var documentIds = documentParticipants.Select(x => x.DocumentId);
            await Task.WhenAll(documentIds.Select(documentId => Groups.AddToGroupAsync(Context.ConnectionId, $"Document{documentId}")));

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            connectionTracker.UntrackConnection(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
