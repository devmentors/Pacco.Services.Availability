using System;
using Convey.MessageBrokers.RabbitMQ;
using Pacco.Services.Availability.Application.Commands;
using Pacco.Services.Availability.Application.Events.Rejected;
using Pacco.Services.Availability.Application.Exceptions;
using Pacco.Services.Availability.Core.Exceptions;

namespace Pacco.Services.Availability.Infrastructure.Exceptions
{
    public class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public object Map(Exception exception, object message)
        {
            switch (exception)
            {
                case CannotExpropriateVipReservationException ex:
                {
                    var command = (ReserveResource) message;
                    return new ReserveResourceRejected(command.Id, command.CustomerId, command.OrderId,
                        command.DateTime, ex.Message, ex.Code);
                }
                case CollidingReservationException ex:
                {
                    var command = (ReserveResource) message;
                    return new ReserveResourceRejected(command.Id, command.CustomerId, command.OrderId,
                        command.DateTime, ex.Message, ex.Code);
                }
                case ResourceNotFoundException ex:
                {
                    switch (message)
                    {
                        case DeleteResource command: 
                            return new DeleteResourceRejected(command.Id, ex.Message, ex.Code);
                        case ReserveResource command:
                            return new ReserveResourceRejected(command.Id, command.CustomerId, command.OrderId, 
                                command.DateTime, ex.Message, ex.Code);
                        case ReleaseResource command:
                            return new ReleaseResourceRejected(command.Id, command.DateTime, ex.Message, ex.Code);
                    }
                }
                break;
            }
            return null;
        }
    }
}