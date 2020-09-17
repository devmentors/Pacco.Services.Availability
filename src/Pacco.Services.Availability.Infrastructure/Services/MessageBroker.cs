using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.MessageBrokers.RabbitMQ;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pacco.Services.Availability.Application.Services;

namespace Pacco.Services.Availability.Infrastructure.Services
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private const string DefaultSpanContextHeader = "span_context";
        private readonly IBusPublisher _busPublisher;
        private readonly ICorrelationContextAccessor _contextAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessagePropertiesAccessor _messagePropertiesAccessor;
        private readonly IMessageOutbox _messageOutbox;
        private readonly ILogger<IMessageBroker> _logger;
        private readonly string _spanContextHeader;

        public MessageBroker(IBusPublisher busPublisher, ICorrelationContextAccessor contextAccessor,
            IHttpContextAccessor httpContextAccessor, IMessagePropertiesAccessor messagePropertiesAccessor,
            RabbitMqOptions options, ILogger<IMessageBroker> logger, IMessageOutbox messageOutbox)
        {
            _busPublisher = busPublisher;
            _contextAccessor = contextAccessor;
            _httpContextAccessor = httpContextAccessor;
            _messagePropertiesAccessor = messagePropertiesAccessor;
            _logger = logger;
            _messageOutbox = messageOutbox;
            _spanContextHeader = string.IsNullOrWhiteSpace(options.SpanContextHeader)
                ? DefaultSpanContextHeader
                : options.SpanContextHeader;
        }

        public Task PublishAsync(params IEvent[] events) => PublishAsync(events?.AsEnumerable());

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            if (events is null)
            {
                return;
            }

            var messageProperties = _messagePropertiesAccessor.MessageProperties;
            var correlationId = messageProperties?.CorrelationId;
            var spanContext = messageProperties?.GetSpanContext(_spanContextHeader);
            var headers = messageProperties.GetHeadersToForward();
            var correlationContext = _contextAccessor.CorrelationContext ??
                                     _httpContextAccessor.GetCorrelationContext();

            foreach (var @event in events)
            {
                if (@event is null)
                {
                    continue;
                }

                var messageId = Guid.NewGuid().ToString("N");
                _logger.LogTrace($"Publishing integration event: {@event.GetType().Name} [id: '{messageId}'].");

                if (_messageOutbox.Enabled)
                {
                    await _messageOutbox.SendAsync(@event, messageId: messageId, correlationId: correlationId,
                        spanContext: spanContext, headers: headers);
                    
                    continue;
                }
                
                await _busPublisher.PublishAsync(@event, messageId, correlationId, spanContext, correlationContext,
                    headers);
            }
        }
    }
}