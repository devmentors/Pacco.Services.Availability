using System;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public class CollidingReservationException : ExceptionBase
    {
        public override string Code => "colliding_reservation";
        
        public CollidingReservationException(Guid customerId) 
            : base($"Reservation for customer: {customerId} cannot be added since it collides with another")
        {
        }
    }
}