using System;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public class CannotExpropriateReservationException : ExceptionBase
    {
        public override string Code => "cannot_expropriate_vip_reservation";
        
        public CannotExpropriateReservationException(Guid resourceId, DateTime dateTime) 
            : base($"Cannot expropriate resource {resourceId} at {dateTime}")
        {
        }
    }
}