using DTOs;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class MessageService : IMessageService
    {

        public Task SendMessageAsync(Message message)
        {
            Console.WriteLine($"Message sent: {message}");
            return Task.CompletedTask;
        }
    }
}
