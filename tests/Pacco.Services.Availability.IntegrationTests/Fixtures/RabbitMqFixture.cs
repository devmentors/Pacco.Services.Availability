using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey;
using Pacco.Services.Availability.Infrastructure.Contexts;
using RabbitMQ.Client;

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

        public async Task PublishAsync<TMessage>(TMessage message, string @namespace = null) where TMessage : class
        {
            await Task.CompletedTask;
//            await _client.PublishAsync(message, ctx => 
//                ctx.UseMessageContext(new CorrelationContext()).UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(@message, @namespace))));
        }
        
        public async Task<TaskCompletionSource<TEntity>> SubscribeAndGetAsync<TMessage, TEntity>(
            Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
        {
            await Task.CompletedTask;
            var taskCompletionSource = new TaskCompletionSource<TEntity>();
            var queueName = Guid.NewGuid().ToString("N");
//            using (var channel = _connection.CreateModel())
//            {
//                channel.QueueBind(queueName, conventions.Exchange, conventions.RoutingKey);
//                channel.BasicQos(0, 1, false);
//                
//                await channel.SubscribeAsync<TMessage>(
//                    async _ => await onMessageReceived(id, taskCompletionSource),
//                    ctx => ctx.UseSubscribeConfiguration(cfg =>
//                        cfg
//                            .FromDeclaredQueue(
//                                builder => builder
//                                    .WithDurability(false)
//                                    .WithName(guid))));
//            }
            
            return taskCompletionSource;
        }
        
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