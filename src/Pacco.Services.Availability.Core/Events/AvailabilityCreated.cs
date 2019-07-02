namespace Pacco.Services.Availability.Core.Events
{
    public class AvailabilityCreated : IDomainEvent
    {
        public Entities.Availability Availability { get; }

        public AvailabilityCreated(Entities.Availability availability)
            => Availability = availability;
    }
}