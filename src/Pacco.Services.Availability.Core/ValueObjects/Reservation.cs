using System;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation : IEquatable<Reservation>
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public bool BelongsToVip { get; private set; }

        public Reservation(DateTime from, DateTime to, Guid customerId, Guid orderId, bool belongsToVip)
        {
            From = from;
            To = to;
            CustomerId = customerId;
            OrderId = orderId;
            BelongsToVip = belongsToVip;
        }

        public bool Equals(Reservation other)
            => CustomerId.Equals(other.CustomerId) && OrderId.Equals(other.OrderId);

        public override bool Equals(object obj)
            => obj is Reservation other && Equals(other);
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (CustomerId.GetHashCode() * 397) ^ OrderId.GetHashCode();
            }
        }
    }
}