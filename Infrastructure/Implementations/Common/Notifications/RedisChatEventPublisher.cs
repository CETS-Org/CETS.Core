using Application.Interfaces.COM;
using DTOs.COM.COM_Chat.Responses;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Implementations.Common.Notifications
{
    public class RedisChatEventPublisher : IChatEventPublisher
    {
        private readonly ISubscriber _subscriber;
        private const string ChannelName = "chat_messages";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisChatEventPublisher(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
            // Sử dụng Lazy connection hoặc connect trực tiếp, ở đây làm giống mẫu cũ của bạn
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _subscriber = connection.GetSubscriber();
        }

        public Task PublishMessageAsync(ChatMessageResponse message)
        {
            if (message == null) return Task.CompletedTask;

            var payload = JsonSerializer.Serialize(message, SerializerOptions);
            return _subscriber.PublishAsync(ChannelName, payload);
        }
    }
}
