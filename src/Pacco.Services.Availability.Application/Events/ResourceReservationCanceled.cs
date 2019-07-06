using System;
using Convey.CQRS.Events;

namespace Pacco.Services.Availability.Application.Events
{
    public class ResourceReservationCanceled : IEvent
    {
        public Guid Id { get; }
        public DateTime DateTime { get; }

        public ResourceReservationCanceled(Guid id, DateTime dateTime)
        {
            Id = id;
            DateTime = dateTime;
        }
    }
}