using CollaboCraft.DataAccess.Repositories;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Block;
using CollaboCraft.Models.BlockImage;
using CollaboCraft.Models.Document;
using CollaboCraft.Services;
using CollaboCraft.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CollaboCraft.Api.Hubs
{
    public class DocumentHub
        (IConnectionTracker connectionTracker, 
        IDocumentService documentService, 
        IBlockService messageService, 
        IBlockRepository blockRepository,
        IBlockImageRepository blockImageRepository,
        IBlockImageService blockImageService,
        IDocumentParticipantService documentParticipantService,
        IUserService userService
        ) : BaseHub
    {
        public async Task CreateDocument(CreateDocumentRequest request)
        {
            try
            {
                request.CreatorId = Id;
                var allUsers = await userService.GetUsers();

                var foundUsers = allUsers
            .Where(u => request.Usernames.Contains(u.Username, StringComparer.OrdinalIgnoreCase))
            .ToList();

                var foundUsernames = foundUsers
                    .Select(u => u.Username)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var notFound = request.Usernames
                    .Where(u => !foundUsernames.Contains(u))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (notFound.Any())
                {
                    throw new HubException($"Пользователи не найдены: {string.Join(", ", notFound)}");
                }

                request.UserIds = await ResolveUserIdsAsync(request.Usernames);
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

        public async Task DeleteDocument(int documentId)
        {
            try
            {
                await documentService.DeleteDocument(documentId, Id);
                await Clients.Group($"Document{documentId}").SendAsync("DocumentDeleted", documentId);

                var connectionIds = connectionTracker.SelectConnectionIds(new List<int> { Id });
                await Task.WhenAll(connectionIds.Select(connectionId => Groups.RemoveFromGroupAsync(connectionId, $"Document{documentId}")));
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

                var allUsers = await userService.GetUsers();

                var foundUsers = allUsers
                    .Where(u => request.Usernames.Contains(u.Username, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                var foundUsernames = foundUsers
                    .Select(u => u.Username)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                var notFound = request.Usernames
                    .Where(u => !foundUsernames.Contains(u))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();

                request.UserIds = await ResolveUserIdsAsync(request.Usernames);

                if (!request.UserIds.Any())
                {
                    throw new HubException("No valid users found.");
                }

                if (!request.UserIds.Any())
                {
                    throw new HubException("No valid users found. Cannot add anyone to the document.");
                }

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

        public async Task RenameDocument(int documentId, string newName)
        {
            try
            {
                await documentService.RenameDocument(documentId, newName, Id);

                await Clients.Group($"Document{documentId}").SendAsync("DocumentRenamed", documentId, newName);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task ChangeUserRoleInDocument(int documentId, int userId, string newRole)
        {
            try
            {
                await documentParticipantService.ChangeUserRoleInDocument(documentId, userId, newRole, Id);
                await Clients.Group($"Document{documentId}").SendAsync("UserRoleChanged", documentId, userId, newRole);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task RemoveUserFromDocument(int documentId, int userId)
        {
            try
            {
                Console.WriteLine($"Remove for userId: {userId}, documentId: {documentId}");
                await documentParticipantService.RemoveUserFromDocument(documentId, userId, Id);

                var connectionIds = connectionTracker.SelectConnectionIds(new List<int> { userId });
                await Task.WhenAll(connectionIds.Select(connectionId => Groups.RemoveFromGroupAsync(connectionId, $"Document{documentId}")));

                Console.WriteLine("Remove successfully");
                await Clients.Group($"Document{documentId}").SendAsync("UserRemoved", documentId, userId);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task<BlockImage> SendBlockImage(SendBlockImageRequest request, FileUpload file)
        {
            Console.WriteLine("➡️ Вызван метод SendBlockImage (Hub)");
            try
            {
                request.UserId = Id;
                Console.WriteLine($"🔹 UserId: {request.UserId}, BlockId: {request.BlockId}");

                var block = await blockRepository.GetBlockById(request.BlockId);
                if (block == null)
                {
                    Console.WriteLine("❌ Блок не найден");
                    throw new Exception($"Блок с ID {request.BlockId} не найден");
                }

                int documentId = block.DocumentId;
                Console.WriteLine($"🔹 DocumentId: {documentId}");

                var blockImage = await blockImageService.SendBlockImage(request, file);

                Console.WriteLine($"✅ Изображение успешно обработано, Id: {blockImage.Id}, Url: {blockImage.Url}");

                await Clients.Group($"Document{documentId}")
                             .SendAsync("ReceiveBlockImage", blockImage);
                Console.WriteLine("📤 Отправлено клиентам в группе");
                return blockImage;
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Ошибка в SendBlockImage (Hub): {e.GetType().Name} - {e.Message}");
                Console.WriteLine($"📄 StackTrace: {e.StackTrace}");
                throw new HubException("Ошибка при обработке изображения: " + e.Message);
            }
        }


        public async Task DeleteBlockImage(int imageId)
        {
            Console.WriteLine("хаб удаления");
            var image = await blockImageRepository.GetImageById(imageId);
            var block = await blockRepository.GetBlockById(image.BlockId);
            int documentId = block.DocumentId;
            try
            {
                await blockImageService.DeleteBlockImage(imageId, Id);
                await Clients.Group($"Document{documentId}")
                             .SendAsync("BlockImageDeleted", imageId);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task<List<BlockImage>> GetBlockImagesByBlock(int blockId)
        {
            try
            {
                var blockImages = await blockImageService.GetBlockImagesByBlock(Id, blockId);
                return blockImages;
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
    

        private async Task<List<int>> ResolveUserIdsAsync(List<string> usernames)
        {
            var allUsers = await userService.GetUsers();
            var userIds = allUsers
                .Where(u => usernames.Contains(u.Username, StringComparer.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToList();

            return userIds;
        }

    }
}
