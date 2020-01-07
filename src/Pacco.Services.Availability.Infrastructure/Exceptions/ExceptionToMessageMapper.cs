using System;
using Convey.MessageBrokers.RabbitMQ;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message) => null;
    }
}