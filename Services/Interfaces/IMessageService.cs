using DTOs.Message.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(CreateMessageRequest message);        //Message AutoMapper DTO to Entity

        //Task<string> ReceiveMessageAsync();
        //Task<IEnumerable<string>> GetAllMessagesAsync();
        //Task DeleteMessageAsync(string messageId);
        //Task UpdateMessageAsync(string messageId, string newMessage);
    }
}
