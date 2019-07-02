using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Core.Events
{
    public class AvailabilityReservationAdded : IDomainEvent
    {
        public Entities.Availability Availability { get; }
        public Reservation Reservation { get; }

        public AvailabilityReservationAdded(Entities.Availability availability, Reservation reservation)
        {
            Availability = availability;
            Reservation = reservation;
        }
    }
}