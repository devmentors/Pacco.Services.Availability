using System;

namespace Pacco.Services.Availability.Core.ValueObjects
{
    public struct Reservation : IEquatable<Reservation>
    {
        public DateTime DateTime { get; }
        public int Priority { get; }

        public Reservation(DateTime dateTimeTime, int priority)
        {
            DateTime = dateTimeTime;
            Priority = priority;
        }

        public bool Equals(Reservation reservation)
            => Priority.Equals(reservation.Priority) && DateTime.Date.Equals(reservation.DateTime.Date);

        public override bool Equals(object obj)
            => obj is Reservation reservation && Equals(reservation);
        
        public override int GetHashCode()
            => DateTime.Date.GetHashCode();
    }
}