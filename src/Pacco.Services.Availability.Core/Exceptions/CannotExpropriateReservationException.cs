using System;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public class CannotExpropriateReservationException : ExceptionBase
    {
        public override string Code => "cannot_expropriate_reservation";
        public Guid ResourceId { get; }
        public DateTime DateTime { get; }

        public CannotExpropriateReservationException(Guid resourceId, DateTime dateTime)
            : base($"Cannot expropriate resource {resourceId} reservation at {dateTime}")
            => (ResourceId, DateTime) = (resourceId, dateTime);
    }
}