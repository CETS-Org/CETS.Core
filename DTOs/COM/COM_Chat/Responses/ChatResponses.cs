using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.COM.COM_Chat.Responses
{
    public class ChatRoomResponse
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string Type { get; set; } = null!;
        public List<string> MemberIds { get; set; } = new();

        // --- THÊM PHẦN NÀY ---
        public List<ChatMemberDetail> Members { get; set; } = new();

        public DateTime LastMessageAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ChatMessageResponse
    {
        public string Id { get; set; } = null!;
        public string RoomId { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public Dictionary<string, string>? Metadata { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ChatMemberDetail
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? AvatarUrl { get; set; }
    }
}
