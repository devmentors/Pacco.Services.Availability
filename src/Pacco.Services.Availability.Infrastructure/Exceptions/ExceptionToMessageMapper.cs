using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events.Rejected;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                ResourceAlreadyExistsException ex => new AddResourceRejected(ex.ResourceId, ex.Message, ex.Code),
                MissingResourceTagsException ex => new AddResourceRejected(((AddResource)message).ResourceId, ex.Message, ex.Code),
                InvalidResourceTagsException ex => new AddResourceRejected(((AddResource)message).ResourceId, ex.Message, ex.Code),
                _ => null
            };
    }
}