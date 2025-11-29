using System.Text.Json;
using Application.Interfaces.COM;
using DTOs.COM.COM_Notification.Responses;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Implementations.Common.Notifications
{
    public class RedisNotificationEventPublisher : INotificationEventPublisher
    {
        private readonly ISubscriber _subscriber;
        private const string ChannelName = "notifications";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        //public RedisNotificationEventPublisher(IConfiguration configuration)
        //{
        //    var redisConnection = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        //    var connection = ConnectionMultiplexer.Connect(redisConnection);
        //    _subscriber = connection.GetSubscriber();
        //}

        public RedisNotificationEventPublisher(IConfiguration configuration)
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

            var redisConnection = configuration["Redis:ConnectionString"]
                                  ?? "localhost:6379,abortConnect=false";

            var connection = ConnectionMultiplexer.Connect(redisConnection);
            _subscriber = connection.GetSubscriber();
        }

        /*public Task PublishNotificationAsync(NotificationResponse notification)
        {
            if (_subscriber == null)
            {
                return Task.CompletedTask;
            }

            var payload = JsonSerializer.Serialize(notification, SerializerOptions);
            return _subscriber.PublishAsync(ChannelName, payload);
        }

        public Task PublishNotificationsAsync(IEnumerable<NotificationResponse> notifications)
        {
            if (_subscriber == null)
            {
                return Task.CompletedTask;
            }

            var tasks = notifications.Select(PublishNotificationAsync);
            return Task.WhenAll(tasks);
        }*/

        public Task PublishNotificationAsync(NotificationResponse notification)
        {
            if (_subscriber == null)
                return Task.CompletedTask;

            var payload = JsonSerializer.Serialize(notification, SerializerOptions);
            return _subscriber.PublishAsync(ChannelName, payload);
        }

        public Task PublishNotificationsAsync(IEnumerable<NotificationResponse> notifications)
        {
            if (_subscriber == null)
                return Task.CompletedTask;

            var tasks = notifications.Select(PublishNotificationAsync);
            return Task.WhenAll(tasks);
        }


    }
}
