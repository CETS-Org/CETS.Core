using DTOs.COM.COM_Chat.Requests;
using DTOs.COM.COM_Chat.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.COM
{
    public interface ICOM_ChatService
    {
        Task<ChatRoomResponse> CreateRoomAsync(CreateChatRoomRequest request);
        Task<List<ChatRoomResponse>> GetUserRoomsAsync(string userId);
        Task<ChatRoomResponse?> GetRoomByIdAsync(string roomId);

        Task<ChatMessageResponse> SendMessageAsync(SendMessageRequest request);
        Task<List<ChatMessageResponse>> GetMessagesAsync(string roomId, int limit = 50, int skip = 0);

        Task UpdateGroupMembersByRoomNameAsync(string roomName, List<string> newMemberIds);
    }
}
