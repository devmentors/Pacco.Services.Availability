using System;

namespace Pacco.Services.Availability.Core.Exceptions
{
    public class CannotExpropriateVipReservationException : ExceptionBase
    {
        public override string Code => "cannot_expropriate_vip_reservation";
        
        public CannotExpropriateVipReservationException(Guid customerId) 
            : base($"Cannot expropriate VIP reservation for customer : {customerId}")
        {
        }
    }
}