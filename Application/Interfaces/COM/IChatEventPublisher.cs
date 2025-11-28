using DTOs.COM.COM_Chat.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.COM
{
    public interface IChatEventPublisher
    {
        Task PublishMessageAsync(ChatMessageResponse message);
    }
}
