using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.MongoDB
{
    public class MongoChatOptions
    {
        public const string SectionName = "Mongo:Chat";
        public string? Database { get; set; }
        public string RoomsCollection { get; set; } = "chat_rooms";
        public string MessagesCollection { get; set; } = "chat_messages";
    }
}
