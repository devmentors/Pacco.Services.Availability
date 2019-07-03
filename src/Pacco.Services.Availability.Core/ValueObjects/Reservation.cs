using System;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation : IEquatable<Reservation>
    {
        public DateTime DateTime { get; }
        public Guid CustomerId { get; }
        public Guid OrderId { get; }
        public bool BelongsToVip { get; }

        public Reservation(DateTime dateTimeTime, Guid customerId, Guid orderId, bool belongsToVip)
        {
            DateTime = dateTimeTime;
            CustomerId = customerId;
            OrderId = orderId;
            BelongsToVip = belongsToVip;
        }

        public bool Equals(Reservation reservation)
            => CustomerId.Equals(reservation.CustomerId) && OrderId.Equals(reservation.OrderId) &&
               DateTime.Date.Equals(reservation.DateTime.Date);

        public override bool Equals(object obj)
            => obj is Reservation reservation && Equals(reservation);
        
        public override int GetHashCode()
            => DateTime.Date.GetHashCode();
    }
}