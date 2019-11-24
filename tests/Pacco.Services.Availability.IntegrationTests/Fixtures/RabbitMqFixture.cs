using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Convey;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace Pacco.Services.Availability.IntegrationTests.Fixtures
{
    public class RabbitMqFixture
    {
        private IConnection _connection;
        private readonly string _defaultNamespace;
        private bool _disposed = false;
        
        public RabbitMqFixture(string defaultNamespace)
        {
            _defaultNamespace = defaultNamespace;
            
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
                VirtualHost = "/",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                UseBackgroundThreadsForIO = true,
                DispatchConsumersAsync = true,
                Ssl = new SslOption()
            };

            _connection = connectionFactory.CreateConnection();
        }

        public Task PublishAsync<TMessage>(TMessage message, string exchange = null) where TMessage : class
        {
            using var channel = _connection.CreateModel();
            var routingKey = SnakeCase(message.GetType().Name);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange, routingKey, body: body, basicProperties: new BasicProperties
            {
                Headers = new Dictionary<string, object>(),
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = Guid.NewGuid().ToString()
            });
            return Task.CompletedTask;
        }
        
        public async Task<TaskCompletionSource<TEntity>> SubscribeAndGetAsync<TMessage, TEntity>(string exchange,
            Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
        {
            await Task.CompletedTask;
            var taskCompletionSource = new TaskCompletionSource<TEntity>();
            using var channel = _connection.CreateModel();
            
            channel.ExchangeDeclare(exchange: exchange,
                durable: true,
                autoDelete: false,
                arguments: null,
                type: "topic");

            var queue = $"test_{SnakeCase(typeof(TMessage).Name)}";

            channel.QueueDeclare(queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.QueueBind(queue, exchange, SnakeCase(typeof(TMessage).Name));
            channel.BasicQos(0, 1, false);
            
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var json = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<TMessage>(json);

                await onMessageReceived(id, taskCompletionSource);
            };
            
            channel.BasicConsume(queue: queue,
                autoAck: true,
                consumer: consumer);
            
            return taskCompletionSource;
        }
        
        private static string SnakeCase(string value)
            => string.Concat(value.Select((x, i) =>
                    i > 0 && value[i - 1] != '.' && value[i - 1] != '/' && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();

        
        
        private string GetRoutingKey<T>(T message, string @namespace = null)
        {
            @namespace = @namespace ?? _defaultNamespace;
            @namespace = string.IsNullOrWhiteSpace(@namespace) ? string.Empty : $"{@namespace}.";

            return $"{@namespace}{typeof(T).Name.Underscore()}".ToLowerInvariant();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _connection.Dispose();
            }

            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}