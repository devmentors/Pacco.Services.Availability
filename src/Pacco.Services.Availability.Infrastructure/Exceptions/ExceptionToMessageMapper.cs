using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Availability.Application.Events;
using Pacco.Services.Availability.Application.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
            => exception switch
            {
                ResourceAlreadyExistsException ex => new AddResourceRejected(ex.Id, ex.Message, ex.Code),
                _ => null
            };
    }
}