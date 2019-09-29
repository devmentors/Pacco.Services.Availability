using System;
using Convey.MessageBrokers.RabbitMQ;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        {
//            switch (exception)
//            {
//                case ResourceAlreadyExistsException ex: return new AddResourceRejected(ex.Id, ex.Message, ex.Code);
//            }

            return null;
        }
    }
}