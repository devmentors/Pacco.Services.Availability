using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Pacco.Services.Availability.Core.Entities;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Instantiation;

namespace Pacco.Services.Availability.IntegrationTests.Fixtures
{
    public class RabbitMqFixture
    {
        private readonly RawRabbit.Instantiation.Disposable.BusClient _client;
        private readonly string _defaultNamespace;
        private bool _disposed = false;
        
        public RabbitMqFixture(string defaultNamespace)
        {
            _defaultNamespace = defaultNamespace;
            
            _client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions()
            {
                ClientConfiguration = new RawRabbitConfiguration
                {
                    Hostnames = new List<string> { "localhost" }, // localhost
                    VirtualHost = "/",
                    Port = 5672,
                    Username = "guest",
                    Password = "guest",
                },
                DependencyInjection = ioc =>
                {
                    ioc.AddSingleton<INamingConventions>(new RabbitMqFixtureNamingConventions("availability"));
                },
                Plugins = p => p  
                    .UseAttributeRouting()
                    .UseRetryLater()
                    .UseMessageContext<CorrelationContext>()
                    .UseContextForwarding()
            });
        }

        public async Task PublishAsync<TMessage>(TMessage message, string @namespace = null) where TMessage : class
        {
            await _client.PublishAsync(message, ctx => 
                ctx.UseMessageContext(CorrelationContext.Empty).UsePublishConfiguration(p => p.WithRoutingKey(GetRoutingKey(@message, @namespace))));
        }
        
        public async Task<TaskCompletionSource<TEntity>> SubscribeAndGetAsync<TMessage, TEntity>(
            Func<Guid, TaskCompletionSource<TEntity>, Task> onMessageReceived, Guid id)
        {
            var taskCompletionSource = new TaskCompletionSource<TEntity>();
            var guid = Guid.NewGuid().ToString();
            
            await _client.SubscribeAsync<TMessage>(
                async _ => await onMessageReceived(id, taskCompletionSource),
                ctx => ctx.UseSubscribeConfiguration(cfg =>
                    cfg
                        .FromDeclaredQueue(
                            builder => builder
                                .WithDurability(false)
                                .WithName(guid))));
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
                _client.Dispose();
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