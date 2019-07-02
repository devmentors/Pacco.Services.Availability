using System;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation : IEquatable<Reservation>
    {
        public DateTime DateTime { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public bool BelongsToVip { get; private set; }

        public Reservation(DateTime dateTimeTime, Guid customerId, Guid orderId, bool belongsToVip)
        {
            DateTime = dateTimeTime;
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