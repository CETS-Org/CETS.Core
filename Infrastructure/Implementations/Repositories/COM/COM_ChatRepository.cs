using Domain.Entities.MongoDB;
using Domain.Interfaces.COM;
using Infrastructure.Implementations.Common.MongoDB;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Repositories.COM
{
    public class COM_ChatRepository : ICOM_ChatRepository
    {
        private readonly IMongoCollection<COM_ChatRoom> _rooms;
        private readonly IMongoCollection<COM_ChatMessage> _messages;

        public COM_ChatRepository(IMongoClient client, IOptions<MongoChatOptions> options)
        {
            var settings = options.Value ?? throw new ArgumentNullException(nameof(options));
            var database = client.GetDatabase(settings.Database);
            _rooms = database.GetCollection<COM_ChatRoom>(settings.RoomsCollection);
            _messages = database.GetCollection<COM_ChatMessage>(settings.MessagesCollection);
        }

        public async Task<COM_ChatRoom> CreateRoomAsync(COM_ChatRoom room)
        {
            await _rooms.InsertOneAsync(room);
            return room;
        }

        public async Task<COM_ChatRoom?> GetRoomByIdAsync(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return null;
            return await _rooms.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<COM_ChatRoom>> GetRoomsByUserIdAsync(string userId)
        {
            // Tìm tất cả phòng mà user là thành viên
            var filter = Builders<COM_ChatRoom>.Filter.AnyEq(x => x.MemberIds, userId);
            return await _rooms.Find(filter)
                .SortByDescending(x => x.LastMessageAt)
                .ToListAsync();
        }

        public async Task<COM_ChatRoom?> GetPrivateRoomByMembersAsync(string user1Id, string user2Id)
        {
            // Tìm phòng private có chứa ĐÚNG và ĐỦ 2 user này
            var builder = Builders<COM_ChatRoom>.Filter;
            var filter = builder.And(
                builder.Eq(x => x.Type, "private"),
                builder.AnyEq(x => x.MemberIds, user1Id),
                builder.AnyEq(x => x.MemberIds, user2Id)
            );
            return await _rooms.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRoomAsync(COM_ChatRoom room)
        {
            var result = await _rooms.ReplaceOneAsync(x => x.Id == room.Id, room);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<COM_ChatMessage> CreateMessageAsync(COM_ChatMessage message)
        {
            await _messages.InsertOneAsync(message);

            // Update LastMessageAt for sorting rooms
            var update = Builders<COM_ChatRoom>.Update.Set(x => x.LastMessageAt, message.CreatedAt);
            await _rooms.UpdateOneAsync(x => x.Id == message.RoomId, update);

            return message;
        }

        public async Task<List<COM_ChatMessage>> GetMessagesByRoomIdAsync(string roomId, int limit, int skip)
        {
            return await _messages.Find(x => x.RoomId == roomId)
                .SortByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync();
        }
    }
}
