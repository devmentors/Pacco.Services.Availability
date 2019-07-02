using Pacco.Services.Availability.Core.ValueObjects;

namespace Pacco.Services.Availability.Core.Events
{
    public class AvailabilityReservationExpropriated : IDomainEvent
    {
        public Entities.Availability Availability { get; }
        public Reservation Reservation { get; }

        public AvailabilityReservationExpropriated(Entities.Availability availability, Reservation reservation)
        {
            Availability = availability;
            Reservation = reservation;
        }
    }
}