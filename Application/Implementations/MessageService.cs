using DTOs.Message.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Implementations
{
    public class MessageService : IMessageService
    {

        public Task SendMessageAsync(CreateMessageRequest message)
        {
            Console.WriteLine($"Message sent: {message}");
            return Task.CompletedTask;
        }
    }
}
