using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.COM.COM_Chat.Requests
{
    public class CreateChatRoomRequest
    {
        public string? Name { get; set; }

        [Required]
        public List<string> MemberIds { get; set; } = new();

        [Required]
        [RegularExpression("^(private|group)$", ErrorMessage = "Type must be 'private' or 'group'")]
        public string Type { get; set; } = "private";
    }

    public class SendMessageRequest
    {
        [Required]
        public string RoomId { get; set; } = null!;

        [Required]
        public string SenderId { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public string Type { get; set; } = "text";

        // Key: "assignmentId", "redirectUrl", "title"
        public Dictionary<string, string>? Metadata { get; set; }
    }

    public class UpdateChatRoomRequest
    {
        public string? Name { get; set; }
        public List<string>? AddMemberIds { get; set; }
        public List<string>? RemoveMemberIds { get; set; }
    }
}
