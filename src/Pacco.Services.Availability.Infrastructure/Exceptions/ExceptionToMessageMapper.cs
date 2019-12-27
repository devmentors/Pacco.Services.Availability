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
                MissingResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message,
                    ex.Code),
                InvalidResourceTagsException ex => new AddResourceRejected(Guid.Empty, ex.Message,
                    ex.Code),
                ResourceAlreadyExistsException ex => new AddResourceRejected(ex.Id, ex.Message,
                    ex.Code),
                CannotExpropriateReservationException ex => message switch
                {
                    ReserveResource command => new ReleaseResourceReservationRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    _ => null
                },
                CustomerNotFoundException ex => message switch
                {
                    ReserveResource command => new ReleaseResourceReservationRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    _ => null
                },
                InvalidCustomerStateException ex => message switch
                {
                    ReserveResource command => new ReleaseResourceReservationRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    _ => null
                },
                ResourceNotFoundException ex => message switch
                {
                    DeleteResource command => new DeleteResourceRejected(command.ResourceId,
                        ex.Message, ex.Code),
                    ReserveResource command => new ReleaseResourceReservationRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    ReleaseResourceReservation command => new ReleaseResourceRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    _ => null
                },
                UnauthorizedResourceAccessException ex => message switch
                {
                    ReserveResource command => new ReleaseResourceReservationRejected(command.ResourceId, command.DateTime,
                        ex.Message, ex.Code),
                    _ => null
                },
                _ => null
            };
    }
}