using Domain.Entities.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.COM
{
    public interface ICOM_ChatRepository
    {
        Task<COM_ChatRoom> CreateRoomAsync(COM_ChatRoom room);
        Task<COM_ChatRoom?> GetRoomByIdAsync(string id);
        Task<List<COM_ChatRoom>> GetRoomsByUserIdAsync(string userId);
        Task<COM_ChatRoom?> GetPrivateRoomByMembersAsync(string user1Id, string user2Id);
        Task<bool> UpdateRoomAsync(COM_ChatRoom room);

        Task<COM_ChatMessage> CreateMessageAsync(COM_ChatMessage message);
        Task<List<COM_ChatMessage>> GetMessagesByRoomIdAsync(string roomId, int limit, int skip);
    }
}
