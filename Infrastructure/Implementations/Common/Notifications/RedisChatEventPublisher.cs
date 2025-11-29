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
            var enabledRaw = configuration["Redis:Enabled"];
            bool enabled = false;

            if (!string.IsNullOrEmpty(enabledRaw))
            {
                enabled = enabledRaw.Equals("true", StringComparison.OrdinalIgnoreCase);
            }

            if (!enabled)
            {
                _subscriber = null!;
                return;
            }

            var connectionString = configuration["Redis:ConnectionString"] 
                ?? configuration.GetConnectionString("Redis") 
                ?? "localhost:6379,abortConnect=false";
            var connection = ConnectionMultiplexer.Connect(connectionString);
            _subscriber = connection.GetSubscriber();
        }

        public Task PublishMessageAsync(ChatMessageResponse message)
        {
            if (message == null || _subscriber == null) return Task.CompletedTask;

            var payload = JsonSerializer.Serialize(message, SerializerOptions);
            return _subscriber.PublishAsync(ChannelName, payload);
        }
    }
}
